using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IViewService
{
    IViewPresenter GetPresenter(object key);

    bool ContainsPresenter(object key);

    void AddPresenter(IViewPresenter presenter, object key);

    Task<IXafBundle<TViewModel>> AddViewAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> AddViewAsync<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;
    Task<IXafBundle> AddViewAsync(Type viewModelType, object key);

    Task AddViewAsync(IXafBundle bundle, object key);

    Task RemoveViewsAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task RemoveViewsAsync(Type viewModelType, object key);

    Task RemoveAllAsync(object key);

    Task RemoveViewAsync<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    Task RemoveViewAsync(IXafBundle bundle, object key);

    Task<IEnumerable<IXafBundle<TViewModel>>> ActivateViewsAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> ActivateFirstViewAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    Task<IXafBundle> ActivateViewAsync(Type viewModelType, object key);

    Task<IXafBundle> ActivateViewAsync(Type viewModelType, object parameter, object key);

    Task ActivateViewAsync(IXafBundle bundle, object key);

    Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, TViewModel viewModel, object key)
        where TViewModel : IXafViewModel<TParameter>;

    Task ActivateViewAsync(IXafBundle bundle, object parameter, object key);

    Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, object key)
        where TViewModel : IXafViewModel<TParameter>;

    Task<IEnumerable<IXafBundle<TViewModel>>> ActivateViewsAsync<TViewModel, TParameter>(TParameter parameter, object key)
        where TViewModel : IXafViewModel<TParameter>;

    Task<IXafBundle<TViewModel>> ActivateFirstViewAsync<TViewModel, TParameter>(TParameter parameter, object key)
        where TViewModel : IXafViewModel<TParameter>;

    Task DeactivateViewsAsync<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    Task DeactivateViewAsync<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    Task DeactivateViewsAsync(Type viewModelType, object key);

    Task DeactivateViewAsync(IXafBundle bundle, object key);
}
