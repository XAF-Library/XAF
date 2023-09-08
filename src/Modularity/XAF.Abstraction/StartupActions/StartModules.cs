using Microsoft.Extensions.Logging;
using XAF.Hosting.Abstraction;

namespace XAF.Modularity.Abstraction.StartupActions;
public class StartModules : IHostStartupAction
{
    private readonly IModuleCatalog _modules;
    private readonly IServiceProvider _services;
    private readonly ILogger<StartModules> _logger;

    public StartModules(IModuleCatalog modules, IServiceProvider services, ILogger<StartModules> logger)
    {
        _modules = modules;
        _services = services;
        _logger = logger;
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.CreateFor<StartModules>()
            .ExecuteAfter<StartHostedServices>();
    }

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
                _logger.LogError(ex, "An error occurred while starting the {moduleName} Module", module.GetName());
            }
        }
    }
}