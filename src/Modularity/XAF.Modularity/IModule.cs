using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XAF.Modularity;

public interface IModule
{
    void Configure(IServiceCollection services, ILoggingBuilder logging, IConfigurationManager configuration, IHostEnvironment environment);

    string GetName();
    Version GetVersion();
}