using XAF.UI.Abstraction.Commands;

namespace XAF.UI.Commands;
public class XafCommand<TParam, TResult> : IXafCommand<TParam, TResult>
{

    private Func<TParam, TResult> _execute;
    private Predicate<TParam?>? _canExecute;

    internal XafCommand(Func<TParam, TResult> execute, Predicate<TParam?>? canExecute)
    {
        _canExecute = canExecute;
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged;
    public event Action<TResult>? Executed;

    public bool CanExecute(TParam? param)
        => _canExecute is null || _canExecute.Invoke(param);


    public bool CanExecute(object? parameter)
        => parameter is not TParam param
            ? throw new InvalidCastException($"Commandparameter can't be casted to: {typeof(TParam)}")
            : CanExecute(param);

    public TResult Execute(TParam param)
        => _execute.Invoke(param);

    public void Execute(object? parameter)
    {
        if (parameter is not TParam param)
        {
            throw new InvalidCastException($"Commandparameter can't be casted to: {typeof(TParam)}");
        }
        var res = Execute(param);
        Executed?.Invoke(res);
    }

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class XafCommand<TParam> : IXafCommand<TParam>
{
    private Action<TParam> _execute;
    private Predicate<TParam?>? _canExecute;

    internal XafCommand(Action<TParam> execute, Predicate<TParam?>? canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(TParam? param)
        => _canExecute is null || _canExecute(param);

    public bool CanExecute(object? parameter)
    {
        if (parameter is null)
        {
            return _canExecute is null || _canExecute(default);
        }

        if (parameter is not TParam param)
        {
            throw new InvalidCastException($"Commandparameter of type {parameter.GetType()} can't be casted to: {typeof(TParam)}");
        }
        return CanExecute(param);
    }

    public void Execute(TParam param)
        => _execute.Invoke(param);

    public void Execute(object? parameter)
    {
        if (parameter is null)
        {
            Execute(default!);
        }

        if (parameter is not TParam param)
        {
            throw new InvalidCastException($"Commandparameter of type {parameter?.GetType()} can't be casted to: {typeof(TParam)}");
        }
        Execute(param);
    }

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}

public class XafCommand : IXafCommand
{
    private Action _execute;
    private Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    internal XafCommand(Action execute, Func<bool> canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute()
        => _canExecute is null || _canExecute.Invoke();

    public bool CanExecute(object? parameter)
        => CanExecute();


    public void Execute()
        => _execute();

    public void Execute(object? parameter)
        => Execute();

    public void RaiseCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    public static IXafCommand<TParam, TResult> CreateCommand<TParam, TResult>(
        Func<TParam, TResult> executeAction,
        Predicate<TParam>? canExecute = null)
        => new XafCommand<TParam, TResult>(executeAction, canExecute);

    public static IXafCommand<TParam> CreateCommand<TParam>(
        Action<TParam> executeAction,
        Predicate<TParam>? canExecute = null)
        => new XafCommand<TParam>(executeAction, canExecute);

    public static IXafCommand CreateCommand(
        Action executeAction,
        Func<bool>? canExecute = null)
        => new XafCommand(executeAction, canExecute);
}