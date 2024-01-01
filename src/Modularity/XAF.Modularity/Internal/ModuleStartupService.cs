using Microsoft.Extensions.Hosting;

namespace XAF.Modularity.Internal;
internal class ModuleStartupService : IHostedService
{
    private readonly IEnumerable<IModuleStartup> _moduleStartups;

    public ModuleStartupService(IEnumerable<IModuleStartup> moduleStartups)
    {
        _moduleStartups = moduleStartups;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var module in _moduleStartups)
        {
            await module.StartAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
