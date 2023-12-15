using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.ViewComposition.Internal;
internal class NavigationService : INavigationService
{
    private readonly IViewService _viewService;

    public NavigationService(IViewService viewService)
    {
        _viewService = viewService;
    }

    public IObservable<bool> CanNavigateBack(object viewPresenterKey)
    {
        throw new NotImplementedException();
    }

    public IObservable<bool> CanNavigateForward(object viewPresenterKey)
    {
        throw new NotImplementedException();
    }

    public Task NavigateBack(object viewPresenterKey)
    {
        throw new NotImplementedException();
    }

    public Task NavigateForward(object viewPresenterKey)
    {
        throw new NotImplementedException();
    }

    public Task NavigateTo<TViewModel>(object viewPresenterKey) where TViewModel : IXafViewModel
    {
        return _viewService.ActivateViewsAsync<TViewModel>(viewPresenterKey);
    }

    public Task NavigateTo<TViewModel, TParameter>(TParameter parameter, object viewPresenterKey)
        where TViewModel : IXafViewModel<TParameter>
    {
        return _viewService.ActivateViewsAsync<TViewModel, TParameter>(parameter, viewPresenterKey);
    }

    public Task NavigateTo<TViewModel>(TViewModel viewModel, object viewPresenterKey) where TViewModel : IXafViewModel
    {
        return _viewService.ActivateViewAsync(viewModel, viewPresenterKey);
    }

    public Task NavigateTo<TViewModel, TParameter>(TViewModel viewModel, TParameter parameter, object viewPresenterKey) where TViewModel : IXafViewModel<TParameter>
    {
        return _viewService.ActivateViewAsync(parameter, viewModel, viewPresenterKey);
    }

    public IObservable<IXafBundle> WhenNavigated(object viewPresenterKey)
    {
        throw new NotImplementedException();
    }
}
