using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfPlugin;
using WpfPluginApp;
using WpfPluginApp.ViewModels;
using XAF.Hosting;
using XAF.Hosting.Abstraction;
using XAF.Modularity.Extensions;
using XAF.UI.WPF.Hosting;

var builder = XafHost.CreateDefaultBuilder(args);

builder.ConfigureWpf<App>();

builder.AddSplashWindow<SplashWindowViewModel>();
await builder.RegisterModuleAsync<WPFModule>(default);

var app = builder.Build();
var uiSyncContext = await app.GetUiSyncContext();

await app.RunAsync();