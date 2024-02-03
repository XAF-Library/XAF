using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XAF.Modularity.Internal;

namespace XAF.Modularity.Extensions;
public static class HostBuilderExtensions
{
    public static void UseModuleStartups(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHostedService<ModuleStartupService>();
    }

    public static void AddModule<T>(this IHostApplicationBuilder builder)
        where T : IModule, new()
    {
        var module = new T();
        module.Configure(builder.Services, builder.Logging, builder.Configuration, builder.Environment);
    }
}
