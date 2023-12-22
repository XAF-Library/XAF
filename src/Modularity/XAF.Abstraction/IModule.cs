namespace XAF.Modularity.Abstraction;

public interface IModule
{
    Task RegisterModuleAsync(IModuleRegistrationContext context, CancellationToken cancellation);
    Task StartModuleAsync(IServiceProvider services, CancellationToken cancellation);

    string GetName();
    Version GetVersion();
}