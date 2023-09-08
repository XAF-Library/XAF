using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace XAF.Hosting.Abstraction;

/// <summary>
/// Several extensions for the <see cref="IServiceCollection"/> and <see cref="IServiceProvider"/>
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Add a startup action to the service collection
    /// </summary>
    /// <typeparam name="TAction">the type of the startup action</typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddStartupAction<TAction>(this IServiceCollection services)
        where TAction : class, IHostStartupAction
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostStartupAction, TAction>());
        return services;
    }

    /// <summary>
    /// Gets an collection of several services
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="types">the collection of service types</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IEnumerable<object> GetServices(this IServiceProvider serviceProvider, IEnumerable<Type> types)
    {
        foreach (var type in types)
        {
            var service = serviceProvider.GetService(type);

            yield return service ?? throw new InvalidOperationException($"{type.FullName} is not registered");
        }
    }
}
