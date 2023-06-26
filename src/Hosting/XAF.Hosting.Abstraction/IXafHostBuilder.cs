using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XAF.Hosting.Abstraction;
public interface IXafHostBuilder : IHostBuilder
{
    IHostEnvironment Environment { get; }

    IServiceCollection Services { get; }

    ConfigurationManager Configuration { get; }

    ILoggingBuilder Logging { get; }
}
