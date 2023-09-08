namespace XAF.Utilities.ExtensionMethods;

/// <summary>
/// Several extensions for a <see cref="Task"/>
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Save wrapper for a fire and forget Async method
    /// </summary>
    /// <param name="task">the task to await</param>
    /// <param name="errorHandler">the error handler for the awaited task</param>
#pragma warning disable S3168 // "async" methods should not return "void"
    public static async void Await(this Task task, IErrorHandler? errorHandler = null)
#pragma warning restore S3168 // "async" methods should not return "void"
    {
        try
        {
            await task;
        }
        catch (Exception ex)
        {
            errorHandler?.HandleException(ex);
        }
    }

    /// <summary>
    /// Save wrapper for a fire and forget Async method
    /// </summary>
    /// <param name="task">the task to await</param>
    /// <param name="completedCallBack">a callback which gets executed if the task completes successfully</param>
    /// <param name="errorHandler">the error handler for the awaited task</param>
#pragma warning disable S3168 // "async" methods should not return "void"
    public static async void Await(this Task task, Action completedCallBack, IErrorHandler? errorHandler = null)
#pragma warning restore S3168 // "async" methods should not return "void"
    {
        try
        {
            await task;
            completedCallBack?.Invoke();
        }
        catch (Exception ex)
        {
            errorHandler?.HandleException(ex);
        }
    }

    /// <summary>
    /// Save wrapper for a fire and forget Async method
    /// </summary>
    /// <param name="task">the task to await</param>
    /// <param name="completedCallBack">a callback which gets executed if the task completes successfully</param>
    /// <param name="errorHandler">the error handler for the awaited task</param>
#pragma warning disable S3168 // "async" methods should not return "void"
    public static async void Await<T>(this Task<T> task, Action<T> completedCallBack, IErrorHandler? errorHandler = null)
#pragma warning restore S3168 // "async" methods should not return "void"
    {
        try
        {
            var result = await task;
            completedCallBack?.Invoke(result);
        }
        catch (Exception ex)
        {
            errorHandler?.HandleException(ex);
        }
    }
}
