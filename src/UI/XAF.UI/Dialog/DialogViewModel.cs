using XAF.UI.Abstraction.Dialog;

namespace XAF.UI.Dialog;
public abstract class DialogViewModel : ViewModelBase, IDialogViewModel
{
    public abstract string Title { get; protected set; }

    public virtual void OnDialogClosed()
    {
    }

    public virtual void OnDialogOpened()
    {
    }
}

public abstract class DialogViewModel<TParameter> : DialogViewModel, IDialogViewModel<TParameter>
{
    public abstract void OnDialogOpend(TParameter parameter);
}

