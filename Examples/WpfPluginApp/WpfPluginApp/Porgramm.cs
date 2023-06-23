using Microsoft.Extensions.Hosting;
using WpfPlugin;
using WpfPluginApp;
using WpfPluginApp.ViewModels;
using WpfPluginApp.Views;
using XAF.Hosting;
using XAF.Modularity.Extensions;
using XAF.UI.WPF.Hosting;

var builder = RxHost.CreateDefaultBuilder(args);

builder.ConfigureWpfApp<App>();
builder.UseSplashWindow<SplashWindowViewModel>();


await builder.RegisterModuleAsync<WPFModule>(default);

var app = builder.Build();
await app.RunAsync();