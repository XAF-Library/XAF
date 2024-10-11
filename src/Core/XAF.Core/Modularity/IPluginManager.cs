using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Core.Modularity;
public interface IPluginManager
{
    void AddPlugin<TPlugin>()
        where TPlugin : IPlugin, new();

    void AddPlugin(Type pluginType);

    void LoadPlugins(IServiceCollection services);

    Task StartPluginsAsync();
}
