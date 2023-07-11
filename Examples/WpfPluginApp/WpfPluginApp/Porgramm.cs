using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WpfPlugin;
using WpfPluginApp;
using WpfPluginApp.ViewModels;
using XAF.Hosting;
using XAF.Modularity.Extensions;
using XAF.UI.Reactive;
using XAF.UI.WPF.Hosting;

var builder = XafHost.CreateDefaultBuilder(args);

builder.AddWpf<App>();
builder.AddSplashWindow<SplashWindowViewModel>();

await builder.RegisterModuleAsync<WPFModule>(default);

var app = builder.Build();

var uiSyncContext = await app.GetUiSyncContext();

app.AddRx(uiSyncContext);

await app.RunAsync();