using System.Reactive;
using System.Windows.Input;

namespace XAF.UI.Reactive.Commands;

public interface IRxCommand<TParam, TResult> : IObservable<TResult>, ICommand, IDisposable
{
    bool IsDisposed { get; }
    new IObservable<bool> CanExecute { get; }

    IObservable<bool> IsExecuting { get; }

    IObservable<TResult> Execute(TParam param);

    IObservable<TResult> Execute();
}

public interface IRxCommand<TResult> : IRxCommand<Unit, TResult>
{

}

public interface IRxCommand : IRxCommand<Unit, Unit>
{

}