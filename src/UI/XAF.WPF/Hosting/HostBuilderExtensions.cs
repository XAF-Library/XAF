using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Windows;
using XAF.Hosting.Abstraction;
using XAF.Modularity.Extensions;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.ViewComposition;
using XAF.UI.WPF.Hosting.Internal;
using XAF.UI.WPF.StartupActions;
using XAF.UI.WPF.ViewComposition;
using XAF.UI.WPF.ViewComposition.Internal;

namespace XAF.UI.WPF.Hosting;
public static class HostBuilderExtensions
{
    public static IXafHostBuilder AddWPFApp(this IXafHostBuilder builder)
    {
        return builder.AddWpfApp<Application>();
    }

    public static IXafHostBuilder AddWpfApp<TApplication>(this IXafHostBuilder builder)
        where TApplication : Application
    {

        builder.Services.AddDefaultUiServices();
        builder.Services.TryAddSingleton<Application, TApplication>();

        builder.Services.AddHostedService<WpfApp>();
        builder.Services.AddStartupAction<WpfShowShell>();
        builder.Services.AddStartupAction<WpfCreateShell>();

        builder.Services.AddSingleton<IHostLifetime, WpfLifetime>();

        builder.Services.TryAddSingleton<IWpfThread, WpfThread>();
        builder.Services.TryAddTransient<IBundleProvider, BundleProvider>();
        builder.Services.TryAddSingleton<IWindowService, WindowService>();
        builder.Services.TryAddSingleton<IViewPresenter, ViewPresenter>();
        builder.ConfigureModularity();

        return builder;
    }

    public static IXafHostBuilder AddSplashWindow<TWindow>(this IXafHostBuilder builder)
        where TWindow : SplashWindow
    {
        builder.Services.AddSingleton<SplashWindow, TWindow>();
        builder.Services.AddStartupAction<WpfShowSplashScreen>();
        return builder;
    }
}
