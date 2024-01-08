using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfPlugin;
using WpfPluginApp;
using WpfPluginApp.ViewModels;
using WpfPluginApp.Views;
using XAF.Modularity.Extensions;
using XAF.UI.WPF.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddWpfApp<App>();

builder.AddModule<WpfModule>();

var app = builder.Build();

await app.RunAsync();