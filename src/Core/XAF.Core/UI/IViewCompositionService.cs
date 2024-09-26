using XAF.Core.MVVM;

namespace XAF.Core.UI;
public interface IViewCompositionService
{
    event AsyncEventHandler<ViewManipulationEventArgs> ViewManipulationRequested;

    event AsyncEventHandler<ViewManipulationEventArgs> ViewManipulationCompleted;

    Task<bool> AddViewAsync<TViewModel>(object presenterKey)
        where TViewModel : class, IXafViewModel;

    Task<bool> AddViewAsync<TViewModel, TParameter>(object presenterKey, TParameter parameter)
        where TViewModel : class, IXafViewModel<TParameter>;

    Task<bool> AddViewAsync<TViewModel>(TViewModel vm, object presenterKey)
        where TViewModel : IXafViewModel;

    Task<bool> AddViewAsync<TViewModel, TParameter>(TViewModel vm, TParameter parameter, object presenterKey)
        where TViewModel : IXafViewModel<TParameter>;

    Task<bool> SelectViewAsync<TViewModel>(TViewModel vm, object presenterKey)
        where TViewModel : IXafViewModel;

    Task<bool> SelectViewAsync<TViewModel, TParameter>(TViewModel vm, TParameter parameter, object presenterKey)
        where TViewModel : IXafViewModel<TParameter>;

    Task<bool> RemoveViewAsync<TViewModel>(TViewModel vm, object presenterKey)
        where TViewModel : IXafViewModel;

    Task<bool> OpenWindowAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task<bool> OpenWindowAsync<TViewModel>(TViewModel vm)
        where TViewModel : IXafViewModel;

    Task<bool> OpenWindowAsync<TViewModel, TParameter>(TParameter parameter)
        where TViewModel : class, IXafViewModel<TParameter>;

    Task<bool> OpenWindowAsync<TViewModel, TParameter>(TViewModel vm, TParameter parameter)
        where TViewModel : IXafViewModel<TParameter>;

    Task<bool> CloseWindowAsync<TViewModel>(TViewModel vm)
        where TViewModel : IXafViewModel;
}

public record ViewManipulationEventArgs(ManipulationType Type, IXafViewModel ViewModel, object PresenterKey, object? Parameter)
{
    public bool Cancle { get; set; }
}
