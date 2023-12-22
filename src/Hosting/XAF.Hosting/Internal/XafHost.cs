using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.Hosting.Abstraction;

namespace XAF.Hosting.Internal;
internal sealed class XafHost : IHost
{
    private readonly ILogger<XafHost> _logger;
    private readonly IHostLifetime _hostLifetime;
    private readonly ApplicationLifetime _applicationLifetime;    

    private volatile bool _stopCalled;

    public IServiceProvider Services { get; }

    public XafHost(IServiceProvider services,
                  IHostApplicationLifetime applicationLifetime,
                  ILogger<XafHost> logger,
                  IHostLifetime hostLifetime)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(applicationLifetime);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(hostLifetime);

        Services = services;

        _applicationLifetime = (applicationLifetime as ApplicationLifetime)!;

        if (_applicationLifetime is null)
        {
            throw new ArgumentException("HostApplicationLifetime type not supported");
        }
        _logger = logger;
        _hostLifetime = hostLifetime;
    }

    public void Dispose()
    {
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Starting App");
        using var combinedCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _applicationLifetime.ApplicationStopping);
        var token = combinedCancellationTokenSource.Token;

        await _hostLifetime.WaitForStartAsync(cancellationToken).ConfigureAwait(false);
        token.ThrowIfCancellationRequested();

        var actions = Services
            .GetRequiredService<OrderedStartupActionCollection>()
            .GetOrderedStartupActions();

        foreach (var startupAction in actions)
        {
            await startupAction.Execute(cancellationToken);
        }

    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_stopCalled)
        {
            return;
        }
        _stopCalled = true;
        _logger.LogDebug("Application stopping");
        _applicationLifetime!.StopApplication();

        var exceptions = new List<Exception>();
        var services = Services.GetServices<IHostedService>();
        foreach (var hostedService in services)
        {
            try
            {
                await hostedService.StopAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        _applicationLifetime.NotifyStopped();

        try
        {
            await _hostLifetime.StopAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }

        if (exceptions.Count > 0)
        {
            var ex = new AggregateException("One or more hosted services failed to stop.", exceptions);
            _logger.LogError(ex, "Application stopped with exceptions");
            throw ex;
        }

        _logger.LogDebug("Application stopped");
    }
}
