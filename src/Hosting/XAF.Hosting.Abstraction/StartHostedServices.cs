using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XAF.Hosting.Abstraction;
public sealed class StartHostedServices : IHostStartupAction
{
    private readonly IEnumerable<IHostedService> _hostedServices;
    private readonly ILogger<StartHostedServices> _logger;

    public StartHostedServices(IEnumerable<IHostedService> hostedServices, ILogger<StartHostedServices> logger)
    {
        _hostedServices = hostedServices;
        _logger = logger;
    }

    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.CreateFor<StartHostedServices>();
    }

    public async Task Execute(CancellationToken cancellationToken = default)
    {
        foreach (var service in _hostedServices)
        {
            _logger.LogDebug("Starting hosted service: {service}", service.GetType().FullName);
            await service.StartAsync(cancellationToken);
        }
    }
}
