using Microsoft.Extensions.Logging;
using XAF.Hosting.Abstraction;
using XAF.Modularity.Abstraction;

namespace XAF.Modularity.StartupActions;
internal class ModuleInitializer : IHostStartupAction
{
    private readonly IModuleCatalog _modules;
    private readonly IServiceProvider _services;
    private readonly ILogger<ModuleInitializer> _logger;

    public ModuleInitializer(IModuleCatalog modules, IServiceProvider services, ILogger<ModuleInitializer> logger)
    {
        _modules = modules;
        _services = services;
        _logger = logger;
    }

    public HostStartupActionExecution ExecutionTime { get; set; } = HostStartupActionExecution.AfterHostedServicesStarted;
    public int Priority => ModuleStartupActionPriorities.ModuleInitialization;

    public async Task Execute(CancellationToken cancellation)
    {
        foreach (var module in _modules)
        {
            try
            {
                await module.StartModuleAsync(_services, cancellation)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error accoured while starting the {moduleName} Module", module.GetName());
            }
        }
    }
}