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

public class WpfModule : Module<ModuleStartup>
{
}

public class ModuleStartup : WpfModuleStartUp
{
    public ModuleStartup(
        IBundleMetadataCollection metadataCollection,
        IViewAdapterCollection adapterCollection,
        IViewService viewService)
        : base(metadataCollection, adapterCollection, viewService)
    {
    }

    protected override async Task ComposeViewsAsync(IViewService viewService)
    {
        await viewService.AddViewAsync<ViewAViewModel>("PageViews");
        await viewService.AddViewAsync<ViewBViewModel>("PageViews");
    }
}
