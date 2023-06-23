using System.Reactive;
using XAF;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Commands;

namespace XAF.UI.Abstraction.Dialog;
public interface IDialogViewModel : IViewModel
{
    IObservable<bool> CanCloseDialog { get; }

    IRxCommand CloseCommand { get; }
}

public interface IDialogViewModel<TResult> : IDialogViewModel
{
    new IRxCommand<TResult> CloseCommand { get; }
}
