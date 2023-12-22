using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfPlugin;
using WpfPluginApp;
using WpfPluginApp.ViewModels;
using WpfPluginApp.Views;
using XAF.Hosting;
using XAF.Hosting.Abstraction;
using XAF.Modularity.Extensions;
using XAF.UI.WPF.Hosting;

var builder = XafHost.CreateDefaultBuilder(args);

builder.AddWpfApp<App>();

builder.AddSplashWindow<SplashScreen>();
await builder.RegisterModuleAsync<WPFModule>(default);
var app = builder.Build();

await app.RunAsync();