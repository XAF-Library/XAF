using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.Modularity;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.Modules.Internal;

namespace XAF.UI.WPF.Modules;
public abstract class WpfModule : Module
{
    public override void Configure(IServiceCollection services, ILoggingBuilder logging, IConfigurationManager configuration, IHostEnvironment environment)
    {
        base.Configure(services, logging, configuration, environment);

        services.AddSingleton<IModuleStartup, DefaultWpfStartup>(s =>
        {
            var assembly = GetType().Assembly;
            var metadataCollection = s.GetRequiredService<IBundleMetadataCollection>();
            var viewAdapterCollection = s.GetRequiredService<IViewAdapterCollection>();
            return new(assembly, metadataCollection, viewAdapterCollection);
        });
    }
}

public abstract class WpfModule<T> : Module<T>
    where T : class, IModuleStartup
{
    public override void Configure(IServiceCollection services, ILoggingBuilder logging, IConfigurationManager configuration, IHostEnvironment environment)
    {
        services.AddSingleton<IModuleStartup, DefaultWpfStartup>(s =>
        {
            var assembly = GetType().Assembly;
            var metadataCollection = s.GetRequiredService<IBundleMetadataCollection>();
            var viewAdapterCollection = s.GetRequiredService<IViewAdapterCollection>();
            return new(assembly, metadataCollection, viewAdapterCollection);
        });
        base.Configure(services, logging, configuration, environment);
    }
}