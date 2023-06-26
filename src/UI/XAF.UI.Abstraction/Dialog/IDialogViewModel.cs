using System.Windows.Input;
using XAF.UI.Abstraction.Commands;

namespace XAF.UI.Abstraction.Dialog;
public interface IDialogViewModel : IViewModel
{
    IObservable<bool> CanCloseDialog { get; }

    ICommand CloseCommand { get; }
}

public interface IDialogViewModel<out TResult> : IDialogViewModel
{
    new IXafResultCommand<TResult> CloseCommand { get; }
}
