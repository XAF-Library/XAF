namespace XAF.Hosting.Abstraction;
public interface IHostStartupAction
{
    /// <summary>
    /// The method that gets Executed while startup.
    /// </summary>
    /// <param name="cancellationToken">the cancellation token to cancle the execution</param>
    /// <returns></returns>
    Task Execute(CancellationToken cancellationToken = default);

    StartupActionOrderRule ConfigureExecutionTime();
}
