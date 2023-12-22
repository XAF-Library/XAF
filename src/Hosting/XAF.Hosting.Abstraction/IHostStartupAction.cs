namespace XAF.Hosting.Abstraction;

/// <summary>
/// An Action that gets called at the host startup
/// </summary>
public interface IHostStartupAction
{
    /// <summary>
    /// The method that gets Executed while startup.
    /// </summary>
    /// <param name="cancellationToken">the cancellation token to cancle the execution</param>
    /// <returns></returns>
    Task Execute(CancellationToken cancellationToken = default);

    /// <summary>
    /// Execution order
    /// </summary>
    /// <returns></returns>
    StartupActionOrderRule ConfigureExecutionTime();
}
