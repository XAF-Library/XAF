using System.Windows.Input;

namespace XAF.UI.Abstraction.Commands;

/// <summary>
/// An extension to the <see cref="ICommand"/> interface to handle typed parameters
/// </summary>
/// <typeparam name="TParam">type of the parameter</typeparam>
public interface IXafCommand<in TParam> : ICommand
{
    /// <summary>
    /// Checks if the command can be executed
    /// </summary>
    /// <param name="param">the typed parameter</param>
    /// <returns></returns>
    bool CanExecute(TParam param);

    /// <summary>
    /// Execute the command
    /// </summary>
    /// <param name="param">the typed parameter</param>
    void Execute(TParam param);

    /// <summary>
    /// Rais the can execute changed event
    /// </summary>
    void RaiseCanExecuteChanged();
}

/// <summary>
/// An extension to the <see cref="ICommand"/> interface to handle typed parameters
/// </summary>
public interface IXafCommand : ICommand
{
    /// <summary>
    /// Checks if the command can be executed
    /// </summary>
    /// <returns></returns>
    bool CanExecute();

    /// <summary>
    /// Execute the command
    /// </summary>
    void Execute();

    /// <summary>
    /// Rais the can execute changed event
    /// </summary>
    void RaiseCanExecuteChanged();
}
