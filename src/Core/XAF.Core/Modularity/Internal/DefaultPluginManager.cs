using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using XAF.Core.Modularity.Attributes;

namespace XAF.Core.Modularity.Internal;
internal class DefaultPluginManager : IModuleManager
{
    private readonly List<IServiceModule> _plugins;
    private readonly Dictionary<IServiceModule, ServiceCollection> _servicesForPlugin;
    private readonly IServiceCollection _exportedServices;
    private readonly Dictionary<IServiceModule, IServiceProvider> _serviceProviderForPlugin;
    private readonly ILoggingBuilder _loggingBuilder;
    private readonly IList<ServiceDescriptor> _hostServices;
    private bool _pluginsLoaded;

    public DefaultPluginManager(IList<ServiceDescriptor> hostServices)
    {
        _plugins = [];
        _exportedServices = new ServiceCollection();
        _hostServices = hostServices;
    }

    public void AddPlugin<TPlugin>() where TPlugin : IServiceModule, new()
    {
        _plugins.Add(new TPlugin());
    }

    public void AddPlugin(Type pluginType)
    {
        if (!pluginType.IsAssignableTo(typeof(IServiceModule)))
        {
            throw new NotSupportedException($"{pluginType} has to implement the {typeof(IServiceModule)}");
        }

        if (pluginType.IsAbstract)
        {
            throw new NotSupportedException($"abstract classes are not supported");
        }

        var ctor = pluginType.GetConstructor(Type.EmptyTypes)
            ?? throw new NotSupportedException($"{pluginType} must have an empty constructor");

        var plugin = (IServiceModule)ctor.Invoke([]);

        _plugins.Add(plugin);
    }

    public void LoadPlugins()
    {
        foreach (var plugin in _plugins)
        {
            var pluginServices = new ServiceCollection { _hostServices };
            _servicesForPlugin.Add(plugin, pluginServices);

            plugin.RegisterServices(pluginServices);

            var loggingBuilder = new LoggingBuilder(pluginServices);
            plugin.ConfigureLogging(loggingBuilder);

            var attributes = plugin.Type.GetCustomAttributes(typeof(ExportsAttribute<>));
            foreach (var attribute in attributes)
            {
                var exportedType = attribute.GetType().GetGenericArguments()[0];

                var descriptor = pluginServices.LastOrDefault(d => d.ServiceType == exportedType)
                    ?? throw new KeyNotFoundException($"Exported type {exportedType} in plugin {plugin} not registered");

                if (_exportedServices.Any(d => d.ServiceType == exportedType))
                {
                    throw new NotSupportedException($"Service {exportedType} is already registered");
                }

                _exportedServices.Add(descriptor);
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

    private class LoggingBuilder : ILoggingBuilder
    {
        public IServiceCollection Services { get; }

        public LoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
