using XAF.UI.Abstraction.Commands;

namespace XAF.UI.Commands;

internal class XafCommand<TParam> : IXafCommand<TParam>
{
    private readonly Action<TParam> _execute;
    private readonly Predicate<TParam>? _canExecute;

    internal XafCommand(Action<TParam> execute, Predicate<TParam>? canExecute)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(TParam param)
        => _canExecute is null || _canExecute(param);

    public bool CanExecute(object? parameter)
    {
        if(parameter is null)
        {
            return CanExecute(default);
        }

        return parameter is not TParam param
            ? throw new InvalidCastException($"Commandparameter of type {parameter.GetType()} can't be casted to: {typeof(TParam)}")
            : CanExecute(param);
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
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    internal XafCommand(Action execute, Func<bool>? canExecute)
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

    public static IXafCommand<TParam> Create<TParam>(
        Action<TParam> executeAction,
        Predicate<TParam>? canExecute = null)
        => new XafCommand<TParam>(executeAction, canExecute);

    public static IXafCommand Create(
        Action executeAction,
        Func<bool>? canExecute = null)
        => new XafCommand(executeAction, canExecute);
}