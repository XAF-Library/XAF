using XAF.Core.MVVM;

namespace XAF.Core.UI;
public interface INavigationService
{

    event AsyncEventHandler<NavigationEventArgs> NavigationRequested;

    event AsyncEventHandler<NavigationEventArgs> NavigationCompleted;

    Task<bool> NavigateToAsync<TViewModel>(object presenterKey)
        where TViewModel : class, IXafViewModel;

    Task<bool> NavigateToAsync<TViewModel>(TViewModel vm, object presenterKey)
        where TViewModel : IXafViewModel;

    Task<bool> NavigateToAsync<TViewModel, TParameter>(TParameter parameter, object presenterKey)
        where TViewModel : IXafViewModel<TParameter>;

    Task<bool> NavigateToAsync<TViewModel, TParameter>(TViewModel vm, TParameter parameter, object presenterKey)
        where TViewModel : class, IXafViewModel<TParameter>;
}

public record NavigationEventArgs(IXafViewModel newVm, IXafViewModel prefVm, object PresenterKey, object? Parameter)
{
    public bool Cancle { get; set; }
}
