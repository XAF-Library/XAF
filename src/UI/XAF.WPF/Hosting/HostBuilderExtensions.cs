using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using System.Windows;
using XAF.Hosting.Abstraction;
using XAF.Modularity.Extensions;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Dialog;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ExtensionMethodes;
using XAF.UI.WPF.Hosting.Internal;
using XAF.UI.WPF.Internal;
using XAF.UI.WPF.StartupActions;
using XAF.UI.WPF.ViewAdapters;
using XAF.UI.WPF.ViewComposition;
using XAF.UI.WPF.ViewComposition.Internal;

namespace XAF.UI.WPF.Hosting;
public static class HostBuilderExtensions
{
    public static IXafHostBuilder UseWPF(this IXafHostBuilder builder)
    {
        return builder.UseWpf<Application>();
    }

    public static IXafHostBuilder UseWpf<TApplication>(this IXafHostBuilder builder)
        where TApplication : Application
    {

        builder.Services.TryAddSingleton<Application, TApplication>();

        builder.Services.AddHostedService<WpfApp>();
        builder.Services.AddStartupAction<WpfShowShell>();

        builder.Services.AddSingleton<IHostLifetime, WpfLifetime>();

        builder.Services.TryAddSingleton<IWpfThread, WpfThread>();
        builder.Services.TryAddTransient<IBundleProvider, BundleProvider>();

        builder.Services.AddTransient<DialogWindow>();
        builder.ConfigureModularity();

        return builder;
    }

    public static IXafHostBuilder AddSplashWindow<TViewModel>(this IXafHostBuilder builder)
        where TViewModel : class, ISplashWindowViewModel
    {
        builder.Services.AddSingleton<ISplashWindowViewModel, TViewModel>();
        builder.Services.AddStartupAction<WpfShowSplashScreen>();
        builder.Services.AddStartupAction<WpfSplashVmExecuteAfterModuleInitialization>();
        return builder;
    }
}
