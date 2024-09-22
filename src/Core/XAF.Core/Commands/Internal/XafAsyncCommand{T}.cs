using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XAF.Core.Commands.Internal;
internal class XafAsyncCommand<TParam> : IXafAsyncCommand<TParam>
{
    private readonly Func<TParam?, Task>? _execute;
    private readonly Func<TParam?, CancellationToken, Task>? _cancelableExecute;
    private readonly Predicate<TParam?>? _canExecute;
    private readonly AsyncCommandOptions _commandOptions;
    private CancellationTokenSource? _cancellationTokenSource;
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler? CanExecuteChanged;

    public XafAsyncCommand(Func<TParam?, Task> execute)
    {
        ArgumentNullException.ThrowIfNull(execute);

        _execute = execute;
    }

    public XafAsyncCommand(Func<TParam?, Task> execute, AsyncCommandOptions options)
    {
        ArgumentNullException.ThrowIfNull(execute);

        _execute = execute;
        _commandOptions = options;
    }

    public XafAsyncCommand(Func<TParam?, CancellationToken, Task> cancelableExecute)
    {
        ArgumentNullException.ThrowIfNull(cancelableExecute);

        _cancelableExecute = cancelableExecute;
    }

    public XafAsyncCommand(Func<TParam?, CancellationToken, Task> cancelableExecute, AsyncCommandOptions options)
    {
        ArgumentNullException.ThrowIfNull(cancelableExecute);

        _cancelableExecute = cancelableExecute;
        _commandOptions = options;
    }

    public XafAsyncCommand(Func<TParam?, Task> execute, Predicate<TParam?> canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _execute = execute;
        _canExecute = canExecute;
    }

    public XafAsyncCommand(Func<TParam?, Task> execute, Predicate<TParam?> canExecute, AsyncCommandOptions options)
    {
        ArgumentNullException.ThrowIfNull(execute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _execute = execute;
        _canExecute = canExecute;
        _commandOptions = options;
    }

    public XafAsyncCommand(Func<TParam?, CancellationToken, Task> cancelableExecute, Predicate<TParam?> canExecute)
    {
        ArgumentNullException.ThrowIfNull(cancelableExecute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _cancelableExecute = cancelableExecute;
        _canExecute = canExecute;
    }

    public XafAsyncCommand(Func<TParam?, CancellationToken, Task> cancelableExecute, Predicate<TParam?> canExecute, AsyncCommandOptions options)
    {
        ArgumentNullException.ThrowIfNull(cancelableExecute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _cancelableExecute = cancelableExecute;
        _canExecute = canExecute;
        _commandOptions = options;
    }

    private Task? _executionTask;

    public Task? ExecutionTask
    {
        get => _executionTask;
        private set
        {
            if (ReferenceEquals(_executionTask, value))
            {
                return;
            }

            _executionTask = value;

            PropertyChanged?.Invoke(this, XafAsyncCommand.ExecutionTaskChangedEventArgs);
            PropertyChanged?.Invoke(this, XafAsyncCommand.IsRunningChangedEventArgs);

            bool isAlreadyCompletedOrNull = value?.IsCompleted ?? true;

            if (_cancellationTokenSource is not null)
            {
                PropertyChanged?.Invoke(this, XafAsyncCommand.CanBeCanceledChangedEventArgs);
                PropertyChanged?.Invoke(this, XafAsyncCommand.IsCancellationRequestedChangedEventArgs);
            }

            if (isAlreadyCompletedOrNull)
            {
                return;
            }

            static async void MonitorTask(XafAsyncCommand<TParam> command, Task task)
            {
                await task;

                if (ReferenceEquals(command._executionTask, task))
                {
                    command.PropertyChanged?.Invoke(command, XafAsyncCommand.ExecutionTaskChangedEventArgs);
                    command.PropertyChanged?.Invoke(command, XafAsyncCommand.IsRunningChangedEventArgs);

                    if (command._cancellationTokenSource is not null)
                    {
                        command.PropertyChanged?.Invoke(command, XafAsyncCommand.CanBeCanceledChangedEventArgs);
                    }

                    if ((command._commandOptions & AsyncCommandOptions.AllowConcurrentExecutions) == 0)
                    {
                        command.CanExecuteChanged?.Invoke(command, EventArgs.Empty);
                    }
                }
            }

            MonitorTask(this, value!);
        }
    }

    public bool CanBeCanceled => IsRunning && _cancellationTokenSource is { IsCancellationRequested: false };
    public bool IsCancellationRequested => _cancellationTokenSource is { IsCancellationRequested: true };
    public bool IsRunning => ExecutionTask is { IsCompleted: false };

    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanExecute(TParam? parameter)
    {
        bool canExecute = _canExecute?.Invoke(parameter) != false;

        return canExecute && ((_commandOptions & AsyncCommandOptions.AllowConcurrentExecutions) != 0 || ExecutionTask is not { IsCompleted: false });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanExecute(object? parameter)
    {
        // Special case, see RelayCommand<T>.CanExecute(object?) for more info
        if (parameter is null && default(TParam) is not null)
        {
            return false;
        }

        if (!XafCommand<TParam>.TryGetCommandArgument(parameter, out TParam? result))
        {
            XafCommand<TParam>.ThrowArgumentExceptionForInvalidCommandArgument(parameter);
        }

        return CanExecute(result);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Execute(TParam? parameter)
    {
        Task executionTask = ExecuteAsync(parameter);

        if ((_commandOptions & AsyncCommandOptions.FlowExceptionsToTaskScheduler) == 0)
        {
            XafAsyncCommand.AwaitAndThrowIfFailed(executionTask);
        }
    }

    public void Execute(object? parameter)
    {
        if (!XafCommand<TParam>.TryGetCommandArgument(parameter, out TParam? result))
        {
            XafCommand<TParam>.ThrowArgumentExceptionForInvalidCommandArgument(parameter);
        }

        Execute(result);
    }

    public Task ExecuteAsync(TParam? parameter)
    {
        Task executionTask;

        if (_execute is not null)
        {
            executionTask = ExecutionTask = _execute(parameter);
        }
        else
        {
            _cancellationTokenSource?.Cancel();

            CancellationTokenSource cancellationTokenSource = cancellationTokenSource = new();
            executionTask = ExecutionTask = _cancelableExecute!(parameter, cancellationTokenSource.Token);
        }

        if ((_commandOptions & AsyncCommandOptions.AllowConcurrentExecutions) == 0)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        return executionTask;
    }

    public Task ExecuteAsync(object? parameter)
    {
        if (!XafCommand<TParam>.TryGetCommandArgument(parameter, out TParam? result))
        {
            XafCommand<TParam>.ThrowArgumentExceptionForInvalidCommandArgument(parameter);
        }

        return ExecuteAsync(result);
    }

    public void Cancel()
    {
        if (_cancellationTokenSource is CancellationTokenSource { IsCancellationRequested: false } cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();

            PropertyChanged?.Invoke(this, XafAsyncCommand.CanBeCanceledChangedEventArgs);
            PropertyChanged?.Invoke(this, XafAsyncCommand.IsCancellationRequestedChangedEventArgs);
        }
    }
}
