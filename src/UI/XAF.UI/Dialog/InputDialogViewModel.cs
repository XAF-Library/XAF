using XAF.UI.Abstraction.Dialog;

namespace XAF.UI.Dialog;
public abstract class InputDialogViewModel<TResult> : ViewModelBase, IInputDialogViewModel<TResult>
{
    public abstract string Title { get; }

    public abstract TResult? OnDialogClosed();

    public abstract void OnDialogOpened();
}

public abstract class InputDialogViewModel<TParameter, TResult> : ViewModelBase, IInputDialogViewModel<TParameter, TResult>
{
    public abstract string Title { get; }

    public abstract TResult? OnDialogClosed();

    public abstract void OnDialogOpened(TParameter parameter);
}
