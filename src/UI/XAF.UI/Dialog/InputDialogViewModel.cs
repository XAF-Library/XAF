using XAF.UI.Abstraction.Dialog;

namespace XAF.UI.Dialog;
public abstract class InputDialogViewModel<TResult> : DialogViewModel, IInputDialogViewModel<TResult>
{
    public abstract TResult? GetResult();

}

public abstract class InputDialogViewModel<TParameter, TResult> : InputDialogViewModel<TResult>, IInputDialogViewModel<TParameter, TResult>
{
    public abstract void OnDialogOpend(TParameter parameter);
}
