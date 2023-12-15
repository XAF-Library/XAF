using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
    private readonly List<IXafBundle> _shells = new();

    public WindowService(IBundleProvider bundleProvider, IWpfThread wpfThread)
    {
        _bundleProvider = bundleProvider;
        _wpfThread = wpfThread;
    }

    public Task CloseAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task CloseAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task ShowAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task ShowAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task ShowDialogAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task ShowDialogAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task ShowDialogAsync<TViewModel, TParameter>(TParameter parameter) where TViewModel : IXafViewModel<TParameter>
    {
        throw new NotImplementedException();
    }

    public Task ShowDialogAsync<TViewModel, TParameter>(TViewModel viewModel, TParameter parameter) where TViewModel : IXafViewModel<TParameter>
    {
        throw new NotImplementedException();
    }

    public async Task ShowShells()
    {
        bool hasShell = false;
        foreach (var bundle in _shells)
        {
            if(bundle.View is not Window window)
            {
                continue;
            }

            bundle.ViewModel.Preload();
            
            _wpfThread.UiDispatcher!.Invoke(() =>
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
}
