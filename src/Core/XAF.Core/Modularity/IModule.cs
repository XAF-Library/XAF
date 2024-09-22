using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace XAF.Core.Modularity;

public interface IModule
{
    string Name { get; }

    Version Version { get; }

    string Description { get; }

    Type Type { get; }

    Assembly Assembly { get; }

    void ConfigureServices(IServiceCollection services);

    Task LoadAsync(IServiceProvider serviceProvider);
}
