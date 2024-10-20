using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using System.Windows;
using XAF.Core.UI;
using XAF.WPF.UI;
using XAF.WPF.UI.Internal;

namespace XAF.WPF.Hosting;
public static class HostingBuilderExtensions
{
    public static IServiceCollection AddWpfApp(this IServiceCollection services)
    {
        services
            .AddWpfServices()
            .AddTransient<Application>();

        return services;
    }

    public static IServiceCollection AddWpfApp<TApp>(this IServiceCollection services)
        where TApp : Application
    {
        services
            .AddWpfServices()
            .AddTransient<Application, TApp>();

        return services;
    }

    public static IServiceCollection AddWpfServices(this IServiceCollection services)
    {
        services.AddSingleton<IViewAdapterLocator, DefaultViewAdapterLocator>()
            .AddSingleton<IViewLocator, DefaultViewLocator>()
            .AddSingleton<IViewCompositionService, DefaultViewCompositionService>()
            .AddSingleton<IViewModelPresenterFactory, DefaultViewModelPresenterFactory>()
            .AddSingleton<ViewModelPresenterLocator>()
            .AddTransient<IViewCollection, ViewCollection>();
        ConsoleLifetime
        return services;
    }
}
