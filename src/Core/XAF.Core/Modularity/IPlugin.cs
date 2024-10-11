using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace XAF.Core.Modularity;

public interface IPlugin
{
    string Name { get; }
    string Description { get; }

    Version Version { get; }

    Type Type { get; }

    Assembly ContainingAssembly { get; }

    void RegisterServices(IServiceCollection services);

    Task StartAsync(IServiceProvider serviceProvider);
}
