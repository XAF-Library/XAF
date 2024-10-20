using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace XAF.Core.Modularity;

public interface IServiceModule
{
    void RegisterServices(IServiceCollection services);

    void ConfigureLogging(ILoggingBuilder loggingBuilder);

    Task StartAsync(IServiceProvider serviceProvider);
}
