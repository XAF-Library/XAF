using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XAF.Hosting.Abstraction;

/// <summary>
/// <inheritdoc/>
/// </summary>
public interface IXafHostBuilder : IHostBuilder
{
    /// <summary>
    /// Gets the hosting environment
    /// </summary>
    IHostEnvironment Environment { get; }

    /// <summary>
    /// Gets the service collection
    /// </summary>
    IServiceCollection Services { get; }


    /// <summary>
    /// Gets the configuration
    /// </summary>
    ConfigurationManager Configuration { get; }


    /// <summary>
    /// Gets the logging builder
    /// </summary>
    ILoggingBuilder Logging { get; }
}
