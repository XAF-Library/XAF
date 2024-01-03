using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WpfPlugin.ViewModels;
using XAF.Modularity;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.Modules;

namespace WpfPlugin;

public class WpfModule : WpfModule<Startup>
{
}

public class Startup : IModuleStartup
{
    private readonly IViewService _viewService;

    public Startup(IViewService viewService)
    {
        _viewService = viewService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _viewService.AddViewAsync<ViewAViewModel>("PageViews");
        await _viewService.AddViewAsync<ViewBViewModel>("PageViews");
    }
}
