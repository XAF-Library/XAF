using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public Task ShowShell()
    {
        _bundleProvider.Cre
    }
}
