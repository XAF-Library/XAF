using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.WPF.ViewComposition.Internal;
internal class WindowService : IWindowService
{
    private readonly IBundleProvider _bundleProvider;

    public WindowService(IBundleProvider bundleProvider)
    {
        _bundleProvider = bundleProvider;
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

    public async Task ShowShell()
    {
        var bundle = _bundleProvider.CreateBundleWithDecoratorAsync<ShellAttribute>();
        var window = bundle.View as Window;

        if (window is null)
        {
            throw new InvalidOperationException("registered shell is not a window");
        }

        bundle.ViewModel.Preload();
        Schedulers.MainScheduler.Schedule(window.Show);
        await bundle.ViewModel.LoadAsync();
    }
}
