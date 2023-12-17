using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction.Commands;
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
