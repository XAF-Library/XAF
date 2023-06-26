namespace XAF.Utilities.ExtensionMethods;

public interface IErrorHandler
{
    void HandleException(Exception exception);
}