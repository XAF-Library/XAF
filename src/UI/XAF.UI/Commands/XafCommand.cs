using XAF.UI.Abstraction.Commands;

namespace XAF.UI.Commands;
public class XafCommand<TParam, TResult> : IXafCommand<TParam, TResult>
{

    private Func<TParam, TResult> _execute;
    private Predicate<TParam>? _canExecute;

    internal XafCommand(Func<TParam, TResult> execute, Predicate<TParam>? canExecute)
    {
        _canExecute = canExecute;
        _execute = execute;
    }

    public event EventHandler? CanExecuteChanged;
    public event Action<TResult>? Executed;

    public bool CanExecute(TParam param)
        => _canExecute is null || _canExecute.Invoke(param);


    public bool CanExecute(object? parameter) 
        => parameter is not TParam param
            ? throw new InvalidCastException($"Commandparameter can't be casted to: {typeof(TParam)}")
            : CanExecute(param);

    public TResult Execute(TParam param) 
        => _execute.Invoke(param);

    public void Execute(object? parameter)
    {
        if(parameter is not TParam param)
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
    private Predicate<TParam>? _canExecute;

    internal XafCommand(Action<TParam> execute, Predicate<TParam>? canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(TParam param)
        => _canExecute is null || _canExecute(param);

    public bool CanExecute(object? parameter)
        => parameter is not TParam param
            ? throw new InvalidCastException($"Commandparameter can't be casted to: {typeof(TParam)}")
            : CanExecute(param);

    public void Execute(TParam param)
        => _execute.Invoke(param);

    public void Execute(object? parameter)
    {
        if (parameter is not TParam param)
        {
            throw new InvalidCastException($"Commandparameter can't be casted to: {typeof(TParam)}");
        }
        Execute(param);
    }
}

public static class XafCommand
{
    public static IXafCommand<TParam, TResult> CreateCommand<TParam, TResult>(
        Func<TParam, TResult> executeAction, 
        Predicate<TParam>? canExecute = null)
        => new XafCommand<TParam, TResult>(executeAction, canExecute);

    public static IXafCommand<TParam> CreateCommand<TParam>(
        Action<TParam> executeAction,
        Predicate<TParam>? canExecute = null)
        => new XafCommand<TParam>(executeAction, canExecute);
}