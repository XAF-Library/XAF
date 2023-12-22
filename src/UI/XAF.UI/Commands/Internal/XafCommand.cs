// This file is inspired from the CommunityToolkit library (CommunityToolkit/dotnet)

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using XAF.UI.Abstraction.Commands;

namespace XAF.UI.Commands.Internal;

internal class XafCommand<TParam> : IXafCommand<TParam>
{
    private readonly Action<TParam?> _execute;
    private readonly Predicate<TParam?>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public XafCommand(Action<TParam?> execute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
    }

    public XafCommand(Action<TParam?> execute, Predicate<TParam?>? canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(TParam? param)
        => _canExecute?.Invoke(param) != false;

    public bool CanExecute(object? parameter)
    {
        if (parameter is null && default(TParam) is not null)
        {
            return false;
        }

        if(!TryGetCommandArgument(parameter, out var commandArgument))
        {
            ThrowArgumentExceptionForInvalidCommandArgument(parameter);
        }

        return CanExecute(commandArgument);
    }

    public void Execute(TParam? param)
    {
        _execute(param);
    }

    public void Execute(object? parameter)
    {
        if(!TryGetCommandArgument(parameter, out var result))
        {
            ThrowArgumentExceptionForInvalidCommandArgument(parameter);
        }

        Execute(result);
    }

    public void NotifyCanExecuteChanged()
    {
        throw new NotImplementedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool TryGetCommandArgument(object? parameter, out TParam? result)
    {
        if (parameter is null && default(TParam) is null)
        {
            result = default;
            return true;
        }

        if (parameter is TParam argument)
        {
            result = argument;
            return true;
        }

        result = default;
        return false;
    }

    [DoesNotReturn]
    internal static void ThrowArgumentExceptionForInvalidCommandArgument(object? parameter)
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        static Exception GetException(object? parameter)
        {
            if (parameter is null)
            {
                return new ArgumentException($"Parameter \"{nameof(parameter)}\" (object) must not be null, as the command type requires an argument of type {typeof(TParam)}.", nameof(parameter));
            }

            return new ArgumentException($"Parameter \"{nameof(parameter)}\" (object) cannot be of type {parameter.GetType()}, as the command type requires an argument of type {typeof(TParam)}.", nameof(parameter));
        }

        throw GetException(parameter);
    }
}

internal class XafCommand : IXafCommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged;

    public XafCommand(Action execute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        _execute = execute;
    }

    public XafCommand(Action execute, Func<bool> canExecute)
    {
        ArgumentNullException.ThrowIfNull(execute);
        ArgumentNullException.ThrowIfNull(canExecute);

        _execute = execute;
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
        => _canExecute is null || _canExecute.Invoke();

    public void Execute(object? parameter)
        => _execute.Invoke();

    public void NotifyCanExecuteChanged()
        => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}