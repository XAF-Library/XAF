using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XAF.Core.Modularity.Attributes;

namespace XAF.Core.Modularity.Internal;
internal class DefaultPluginManager : IPluginManager
{
    private readonly List<IPlugin> _plugins;
    private readonly Dictionary<IPlugin, ServiceCollection> _servicesForPlugin;
    private readonly Dictionary<IPlugin, IServiceProvider> _serviceProviderForPlugin;
    private bool _pluginsLoaded;
    private IServiceCollection _hostServices;

    public DefaultPluginManager()
    {
        _plugins = [];
    }

    public void AddPlugin<TPlugin>() where TPlugin : IPlugin, new()
    {
        _plugins.Add(new TPlugin());
    }

    public void AddPlugin(Type pluginType)
    {
        if (!pluginType.IsAssignableTo(typeof(IPlugin)))
        {
            throw new NotSupportedException($"{pluginType} has to implement the {typeof(IPlugin)}");
        }

        if (pluginType.IsAbstract)
        {
            throw new NotSupportedException($"abstract classes are not supported");
        }

        var ctor = pluginType.GetConstructor(Type.EmptyTypes) 
            ?? throw new NotSupportedException($"{pluginType} must have an empty constructor");

        var plugin = (IPlugin)ctor.Invoke([]);

        _plugins.Add(plugin);
    }

    public void LoadPlugins(IServiceCollection services)
    {
        _hostServices = services;
        foreach (var plugin in _plugins) 
        {
            var collection = new ServiceCollection();
            _servicesForPlugin.Add(plugin, collection);
            plugin.RegisterServices(collection);

            var attributes =  plugin.Type.GetCustomAttributes(typeof(ExportsAttribute<>));
            foreach (var attribute in attributes)
            {
                var exportedType = attribute.GetType().GetGenericArguments()[0];

                var descriptor = collection.LastOrDefault(d => d.ServiceType == exportedType) 
                    ?? throw new KeyNotFoundException($"Exported type {exportedType} in plugin {plugin} not registered");
                
                if (collection.Any(d => d.ServiceType == exportedType))
                {
                    throw new NotSupportedException($"Service {exportedType} is already registered");
                }

                collection.Add(descriptor);
            }

        }
        _pluginsLoaded = true;
    }

    public Task StartPluginsAsync()
    {
        if (!_pluginsLoaded)
        {
            return Task.FromException(new InvalidOperationException("Plugins not loaded. Please call 'LoadPlugins' first."));
        }

        var tasks = new Task[_plugins.Count];

        for (int i = 0; i < tasks.Length; i++)
        {
            var plugin = _plugins[i];
            var collection = _servicesForPlugin[plugin];
            collection.Add(_hostServices.Where(d => d.ServiceType != typeof(ILoggerFactory)));
            var provider = collection.BuildServiceProvider();
            _serviceProviderForPlugin[plugin] = provider;
            tasks[i] = plugin.StartAsync(provider);
        }

        return Task.WhenAll(tasks);

    }
}
