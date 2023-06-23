using System.Reactive;
using XAF;
using XAF.Commands;

namespace XAF.Dialog;
public interface IDialogViewModel : IViewModel
{
    IObservable<bool> CanCloseDialog { get; }

    IRxCommand CloseCommand { get; }
}

public interface IDialogViewModel<TResult> : IDialogViewModel
{
    new IRxCommand<TResult> CloseCommand { get; }
}
