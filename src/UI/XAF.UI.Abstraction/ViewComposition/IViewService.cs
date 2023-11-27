using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IViewService
{
    IViewPresenter GetPresenter(object key);

    void AddPresenter(IViewPresenter presenter, object key);

    Task AddViewAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task AddViewAsync<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    Task RemoveViewsAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task RemoveViewAsync<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    Task ActivateViewsAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task ActivateViewAsync<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    Task ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, TViewModel viewModel, object key)
        where TViewModel : IXafViewModel<TParameter>;

    Task ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, object key)
        where TViewModel : IXafViewModel<TParameter>;

    Task ActivateViewsAsync<TViewModel, TParameter>(TParameter parameter, object key)
        where TViewModel : IXafViewModel<TParameter>;

    Task DeactivateViewsAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task DeactivateViewAsync<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;
}
