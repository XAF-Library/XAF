using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace XAF.Modularity;
public abstract class Module : IModule
{
    public virtual void Configure(IServiceCollection services, ILoggingBuilder logging, IConfigurationManager configuration, IHostEnvironment environment)
    {
        ConfigureServices(services, environment);
        ConfigureLogging(logging, environment);
        ConfigureConfiguration(configuration, environment);
    }

    protected virtual void ConfigureServices(IServiceCollection services, IHostEnvironment environment) { }

    protected virtual void ConfigureLogging(ILoggingBuilder logging, IHostEnvironment environment) { }

    protected virtual void ConfigureConfiguration(IConfigurationManager configuration, IHostEnvironment environment) { }

    public virtual string GetName()
    {
        return GetType().FullName!;
    }

    public Version GetVersion()
    {
        return Assembly.GetAssembly(GetType())?.GetName().Version!;
    }
}

public abstract class Module<T> : Module
    where T : class, IModuleStartup
{
    public override void Configure(IServiceCollection services, ILoggingBuilder logging, IConfigurationManager configuration, IHostEnvironment environment)
    {
        base.Configure(services, logging, configuration, environment);
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IModuleStartup, T>());
    }
}
