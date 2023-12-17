using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;
using XAF.UI.WPF.Hosting;

namespace XAF.UI.WPF.ViewComposition.Internal;
internal class WindowService : IWindowService
{
    private readonly IBundleProvider _bundleProvider;
    private readonly IWpfThread _wpfThread;
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IXafBundle> _shells = new();
    private readonly List<IXafBundle> _openWindows = new();
    private Type _defaultWindowType = typeof(Window);

    public WindowService(IBundleProvider bundleProvider, IWpfThread wpfThread, IServiceProvider serviceProvider)
    {
        _bundleProvider = bundleProvider;
        _wpfThread = wpfThread;
        _serviceProvider = serviceProvider;
    }

    public async Task CloseAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        var toClose = _openWindows.Where(b => b.Metadata.ViewModelType == typeof(TViewModel));
        foreach (var bundle in toClose)
        {
            if (bundle.View is not Window window)
            {
                continue;
            }

            window.Close();
            await bundle.ViewModel.Unload().ConfigureAwait(false);
        }
    }

    public async Task CloseAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        var bundle = _openWindows.FirstOrDefault(b => b.ViewModel.Equals(viewModel));
        if (bundle is null)
        {
            return;
        }

        if (bundle.View is not Window window)
        {
            return;
        }

        window.Close();
        await bundle.ViewModel.Unload();
    }

    public async Task ShowAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        var bundle = await _bundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        await ShowAsync(bundle);
    }

    public async Task ShowAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        var bundle = await _bundleProvider.CreateBundleAsync(viewModel).ConfigureAwait(false);
        await ShowAsync(bundle);
    }

    public async Task ShowAsync(IXafBundle bundle)
    {
        if (bundle.View is not Window bundleWindow)
        {
            bundleWindow = (Window)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, _defaultWindowType);
            bundleWindow.Content = bundle.View;
        }

        bundle.ViewModel.Prepare();
        Schedulers.MainScheduler.Schedule(() => bundleWindow.Show());
        await bundle.ViewModel.LoadAsync().ConfigureAwait(false);

    }

    public async Task ShowDialogAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        var bundle = await _bundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        await ShowDialogAsync(bundle);
    }

    public async Task ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        var bundle = await _bundleProvider.CreateBundleAsync(viewModel).ConfigureAwait(false);
        await ShowDialogAsync(bundle);
    }

    public async Task ShowDialogAsync<TViewModel, TParameter>(TParameter parameter) where TViewModel : IXafViewModel<TParameter>
    {
        var bundle = await _bundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        await ShowDialogAsync(bundle, parameter);
    }

    public async Task ShowDialogAsync<TViewModel, TParameter>(TViewModel viewModel, TParameter parameter) where TViewModel : IXafViewModel<TParameter>
    {
        var bundle = await _bundleProvider.CreateBundleAsync(viewModel).ConfigureAwait(false);

        await ShowDialogAsync(bundle, parameter);
    }

    public async Task ShowDialogAsync(IXafBundle bundle)
    {
        if (bundle.View is not Window bundleWindow)
        {
            bundleWindow = (Window)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, _defaultWindowType);
            bundleWindow.Content = bundle.View;
        }

        bundle.ViewModel.Prepare();
        Schedulers.MainScheduler.Schedule(() => bundleWindow.ShowDialog());
        await bundle.ViewModel.LoadAsync().ConfigureAwait(false);
    }

    public async Task ShowDialogAsync<TParameter>(IXafBundle bundle, TParameter parameter)
    {
        if (bundle.View is not Window bundleWindow)
        {
            bundleWindow = (Window)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, _defaultWindowType);
            bundleWindow.Content = bundle.View;
        }

        var vm = (IXafViewModel<TParameter>)bundle.ViewModel
            ?? throw new Exception("Wrong type");

        vm.Prepare();
        vm.Prepare(parameter);
        Schedulers.MainScheduler.Schedule(() => bundleWindow.ShowDialog());
        await bundle.ViewModel.LoadAsync().ConfigureAwait(false);
    }

    public async Task ShowShells()
    {
        bool hasShell = false;
        foreach (var bundle in _shells)
        {
            if (bundle.View is not Window window)
            {
                continue;
            }

            bundle.ViewModel.Prepare();

            Schedulers.MainScheduler.Schedule(() =>
            {
                window.Show();
                _wpfThread.Application!.MainWindow = window;
            });

            await bundle.ViewModel.LoadAsync();
            hasShell = true;
        }

        if (!hasShell)
        {
            throw new InvalidOperationException("No shell window found");
        }

    }

    public async Task CreateShells()
    {
        var bundles = _bundleProvider.CreateBundlesWithDecoratorAsync<ShellAttribute>();
        await foreach (var bundle in bundles)
        {
            _shells.Add(bundle);
        }
    }

    public void SetDefaultWindowType<TWindow>() where TWindow : class
    {
        SetDefaultWindowType(typeof(TWindow));
    }

    public void SetDefaultWindowType(Type type)
    {
        if (!type.IsAssignableTo(typeof(Window)))
        {
            throw new InvalidOperationException($"Window must be of type {typeof(Window)}");
        }

        _defaultWindowType = type;
    }
}
