using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace XAF.Modularity.Abstraction;
public static class ModuleContextExtensions
{
    public static IServiceCollection GetServiceCollection(this IModuleRegistrationContext registrationContext)
        => registrationContext.Get<IServiceCollection>();

    public static IHostEnvironment GetEnvironment(this IModuleRegistrationContext registrationContext)
        => registrationContext.Get<IHostEnvironment>();

    public static ConfigurationManager GetConfiguration(this IModuleRegistrationContext registrationContext)
        => registrationContext.Get<ConfigurationManager>();

    public static ILoggingBuilder GetLogging(this IModuleRegistrationContext registrationContext)
        => registrationContext.Get<ILoggingBuilder>();
}
