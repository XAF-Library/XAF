namespace XAF.Extensions;

public interface IErrorHandler
{
    void HandleException(Exception exception);
}