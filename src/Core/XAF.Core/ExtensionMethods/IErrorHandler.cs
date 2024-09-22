namespace XAF.Core.ExtensionMethods;

/// <summary>
/// Interface to handle errors
/// </summary>
public interface IErrorHandler
{
    /// <summary>
    /// Gets executed if an exception occurred
    /// </summary>
    /// <param name="exception"></param>
    void HandleException(Exception exception);
}