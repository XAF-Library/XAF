using System.ComponentModel;

namespace XAF.Core.Commands;
public interface IXafAsyncCommand : IXafCommand, INotifyPropertyChanged
{
    Task? ExecutionTask { get; }

    bool CanBeCanceled { get; }

    bool IsCancellationRequested { get; }

    bool IsRunning { get; }

    Task ExecuteAsync(object? parameter);

    void Cancel();
}

public interface IXafAsyncCommand<in TParameter> : IXafAsyncCommand, IXafCommand<TParameter>
{
    Task ExecuteAsync(TParameter? parameter);
}
