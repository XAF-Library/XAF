using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XAF.Core.Modularity;
using XAF.Core.Modularity.Internal;

namespace XAF.Core.Hosting;
public static class HostBuilderExtensions
{

    public static IHostApplicationBuilder AddDefaultPluginManager(this IHostApplicationBuilder builder)
    {

        var pluginManager = new DefaultPluginManager(builder.Services);

        builder.Services.AddSingleton<IModuleManager>(pluginManager);
        builder.Properties.Add("PluginManager", pluginManager);
        return builder;
    }

    public static IHostApplicationBuilder UseXaf(this IHostApplicationBuilder builder)
    {
        builder.AddDefaultPluginManager();
        return builder;
    }
}
