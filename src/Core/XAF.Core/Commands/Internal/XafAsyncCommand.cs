using System.ComponentModel;

namespace XAF.Core.Commands.Internal;
internal class XafAsyncCommand : IXafAsyncCommand
{
    private readonly Func<Task>? _execute;
    private readonly Func<CancellationToken, Task>? _cancelableExecute;
    private readonly Func<bool>? _canExecute;
    private readonly AsyncCommandOptions _commandOptions;

    private CancellationTokenSource? _cancellationTokenSource;

    public XafAsyncCommand(Func<Task> execute)
    {
        ArgumentNullException.ThrowIfNull(execute);

        _execute = execute;
    }

    public XafAsyncCommand(Func<Task> execute, AsyncCommandOptions commandOptions)
    {
        ArgumentNullException.ThrowIfNull(execute);

        _execute = execute;
        _commandOptions = commandOptions;
    }

    public XafAsyncCommand(Func<CancellationToken, Task> cancelableExecute)
    {
        ArgumentNullException.ThrowIfNull(cancelableExecute);

        _cancelableExecute = cancelableExecute;
    }

    public XafAsyncCommand(Func<CancellationToken, Task> cancelableExecute, AsyncCommandOptions commandOptions)
    {
        ArgumentNullException.ThrowIfNull(cancelableExecute);
        _cancelableExecute = cancelableExecute;
        _commandOptions = commandOptions;
    }

    public XafAsyncCommand(Func<Task> execute, Func<bool> canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _execute = execute;
        _canExecute = canExecute;
    }

    public XafAsyncCommand(Func<Task> execute, Func<bool> canExecute, AsyncCommandOptions commandOptions)
    {
        ArgumentNullException.ThrowIfNull(execute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _execute = execute;
        _canExecute = canExecute;
        _commandOptions = commandOptions;
    }

    public XafAsyncCommand(Func<CancellationToken, Task> cancelableExecute, Func<bool> canExecute)
    {
        ArgumentNullException.ThrowIfNull(cancelableExecute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _cancelableExecute = cancelableExecute;
        _canExecute = canExecute;
    }

    public XafAsyncCommand(Func<CancellationToken, Task> cancelableExecute, Func<bool> canExecute, AsyncCommandOptions commandOptions)
    {
        ArgumentNullException.ThrowIfNull(cancelableExecute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _cancelableExecute = cancelableExecute;
        _canExecute = canExecute;
        _commandOptions = commandOptions;
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

            PropertyChanged?.Invoke(this, ExecutionTaskChangedEventArgs);
            PropertyChanged?.Invoke(this, IsRunningChangedEventArgs);

            bool isAlreadyCompletedOrNull = value?.IsCompleted ?? true;

            if (_cancellationTokenSource is not null)
            {
                PropertyChanged?.Invoke(this, CanBeCanceledChangedEventArgs);
                PropertyChanged?.Invoke(this, IsCancellationRequestedChangedEventArgs);
            }

            if (isAlreadyCompletedOrNull)
            {
                return;
            }

            static async void MonitorTask(XafAsyncCommand command, Task task)
            {
                await task;

                if (ReferenceEquals(command._executionTask, task))
                {
                    command.PropertyChanged?.Invoke(command, ExecutionTaskChangedEventArgs);
                    command.PropertyChanged?.Invoke(command, IsRunningChangedEventArgs);

                    if (command._cancellationTokenSource is not null)
                    {
                        command.PropertyChanged?.Invoke(command, CanBeCanceledChangedEventArgs);
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

    public event EventHandler? CanExecuteChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    public void Cancel()
    {
        if (_cancellationTokenSource is CancellationTokenSource { IsCancellationRequested: false } cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();

            PropertyChanged?.Invoke(this, CanBeCanceledChangedEventArgs);
            PropertyChanged?.Invoke(this, IsCancellationRequestedChangedEventArgs);
        }
    }

    public bool CanExecute(object? parameter)
    {
        var canExecute = _canExecute?.Invoke() != false;

        return canExecute && ((_commandOptions & AsyncCommandOptions.AllowConcurrentExecutions) != 0 || ExecutionTask is not { IsCompleted: false });
    }

    public void Execute(object? parameter)
    {
        var executionTask = ExecuteAsync(parameter);

        if ((_commandOptions & AsyncCommandOptions.FlowExceptionsToTaskScheduler) == 0)
        {
            AwaitAndThrowIfFailed(executionTask);
        }
    }

    public Task ExecuteAsync(object? parameter)
    {
        Task executionTask;

        if (_execute is not null)
        {
            executionTask = ExecutionTask = _execute();
        }
        else
        {
            _cancellationTokenSource?.Cancel();

            CancellationTokenSource cancellationTokenSource = _cancellationTokenSource = new();

            executionTask = ExecutionTask = _cancelableExecute!(cancellationTokenSource.Token);
        }

        if ((_commandOptions & AsyncCommandOptions.AllowConcurrentExecutions) == 0)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        return executionTask;
    }

    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    internal static async void AwaitAndThrowIfFailed(Task executionTask)
    {
        await executionTask;
    }

    internal static readonly PropertyChangedEventArgs ExecutionTaskChangedEventArgs = new(nameof(ExecutionTask));
    internal static readonly PropertyChangedEventArgs CanBeCanceledChangedEventArgs = new(nameof(CanBeCanceled));
    internal static readonly PropertyChangedEventArgs IsCancellationRequestedChangedEventArgs = new(nameof(IsCancellationRequested));
    internal static readonly PropertyChangedEventArgs IsRunningChangedEventArgs = new(nameof(IsRunning));
}

[Flags]
public enum AsyncCommandOptions
{
    None = 0,
    AllowConcurrentExecutions = 1 << 0,
    FlowExceptionsToTaskScheduler = 1 << 1,
}