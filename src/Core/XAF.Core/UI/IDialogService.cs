using XAF.Core.MVVM;

namespace XAF.Core.UI;
public interface IDialogService
{
    event AsyncEventHandler<DialogEventArgs> DialogRequested;

    event AsyncEventHandler<DialogEventArgs> DialogOpened;

    event AsyncEventHandler<DialogEventArgs> DialogClosed;

    Task<TResult?> ShowDialogAsync<TResult, TViewModel>()
        where TViewModel : class, IXafDialogViewModel<TResult>;

    Task<TResult?> ShowDialogAsync<TResult, TViewModel>(TViewModel vm)
        where TViewModel : class, IXafDialogViewModel<TResult>;

    Task<TResult?> ShowDialogAsync<TResult, TViewModel, TParameter>(TParameter parameter)
        where TViewModel : class, IXafDialogViewModel<TResult, TParameter>;

    Task<TResult?> ShowDialogAsync<TResult, TViewModel, TParameter>(TViewModel vm, TParameter parameter)
        where TViewModel : class, IXafDialogViewModel<TResult, TParameter>;
}

public record DialogEventArgs(IXafViewModel ViewModel, object? Parameter, object Result?)
{
    bool Cancle { get; set; }
}
