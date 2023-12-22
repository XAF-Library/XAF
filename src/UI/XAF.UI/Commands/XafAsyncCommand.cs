using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.Commands;
using XAF.UI.Commands.Internal;

namespace XAF.UI.Commands;
public static class XafCommand
{
    public static IXafCommand Create(Action execute)
        => new Internal.XafCommand(execute);

    public static IXafCommand Create(Action execute, Func<bool> canExecute)
        => new Internal.XafCommand(execute, canExecute);

    public static IXafCommand<T> Create<T>(Action<T?> execute)
        => new XafCommand<T>(execute);

    public static IXafCommand<T> Create<T>(Action<T?> execute, Predicate<T?> canExecute)
        => new XafCommand<T>(execute, canExecute);

    public static IXafAsyncCommand Create(Func<Task> execute)
        => new XafAsyncCommand(execute);

    public static IXafAsyncCommand Create(Func<Task> execute, AsyncCommandOptions commandOptions)
        => new XafAsyncCommand(execute, commandOptions);

    public static IXafAsyncCommand Create(Func<CancellationToken, Task> execute)
        => new XafAsyncCommand(execute);

    public static IXafAsyncCommand Create(Func<CancellationToken, Task> execute, AsyncCommandOptions commandOptions)
        => new XafAsyncCommand(execute, commandOptions);

    public static IXafAsyncCommand Create(Func<Task> execute, Func<bool> canExecute)
        => new XafAsyncCommand(execute, canExecute);

    public static IXafAsyncCommand Create(Func<Task> execute, Func<bool> canExecute, AsyncCommandOptions commandOptions)
        => new XafAsyncCommand(execute, canExecute, commandOptions);

    public static IXafAsyncCommand Create(Func<CancellationToken, Task> execute, Func<bool> canExecute)
        => new XafAsyncCommand(execute, canExecute);

    public static IXafAsyncCommand Create(Func<CancellationToken, Task> execute, Func<bool> canExecute, AsyncCommandOptions commandOptions)
        => new XafAsyncCommand(execute, canExecute, commandOptions);

    public static IXafAsyncCommand<T> Create<T>(Func<T?, Task> execute)
       => new XafAsyncCommand<T>(execute);

    public static IXafAsyncCommand<T> Create<T>(Func<T?, Task> execute, AsyncCommandOptions commandOptions)
        => new XafAsyncCommand<T>(execute, commandOptions);

    public static IXafAsyncCommand<T> Create<T>(Func<T?, CancellationToken, Task> execute)
        => new XafAsyncCommand<T>(execute);

    public static IXafAsyncCommand<T> Create<T>(Func<T?, CancellationToken, Task> execute, AsyncCommandOptions commandOptions)
        => new XafAsyncCommand<T>(execute, commandOptions);

    public static IXafAsyncCommand<T> Create<T>(Func<T?, Task> execute, Predicate<T?> canExecute)
        => new XafAsyncCommand<T>(execute, canExecute);

    public static IXafAsyncCommand<T> Create<T>(Func<T?, Task> execute, Predicate<T?> canExecute, AsyncCommandOptions commandOptions)
        => new XafAsyncCommand<T>(execute, canExecute, commandOptions);

    public static IXafAsyncCommand<T> Create<T>(Func<T?, CancellationToken, Task> execute, Predicate<T?> canExecute)
        => new XafAsyncCommand<T>(execute, canExecute);

    public static IXafAsyncCommand<T> Create<T>(Func<T?, CancellationToken, Task> execute, Predicate<T?> canExecute, AsyncCommandOptions commandOptions)
        => new XafAsyncCommand<T>(execute, canExecute, commandOptions);
}
