using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XAF.Hosting.Abstraction;

/// <summary>
/// A startup service that starts all registered hosted services
/// </summary>
public sealed class StartHostedServices : IHostStartupAction
{
    private readonly IEnumerable<IHostedService> _hostedServices;
    private readonly ILogger<StartHostedServices> _logger;

    /// <summary>
    /// A startup service that starts all registered hosted services
    /// </summary>
    /// <param name="hostedServices">the list of hosted services</param>
    /// <param name="logger">a logger for the startup action</param>
    public StartHostedServices(IEnumerable<IHostedService> hostedServices, ILogger<StartHostedServices> logger)
    {
        _hostedServices = hostedServices;
        _logger = logger;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public StartupActionOrderRule ConfigureExecutionTime()
    {
        return StartupActionOrderRule.Create();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public async Task Execute(CancellationToken cancellationToken = default)
    {
        foreach (var service in _hostedServices)
        {
            _logger.LogDebug("Starting hosted service: {service}", service.GetType().FullName);
            await service.StartAsync(cancellationToken);
        }
    }
}
