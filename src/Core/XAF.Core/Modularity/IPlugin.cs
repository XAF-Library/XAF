using Microsoft.Extensions.DependencyInjection;

namespace XAF.Core.Modularity;

public interface IPlugin
{
    string Name { get; }

    Version Version { get; }

    string Description { get; }

    void RegisterServices(IServiceCollection services);

    Task LoadAsync(IServiceProvider serviceProvider);
}
