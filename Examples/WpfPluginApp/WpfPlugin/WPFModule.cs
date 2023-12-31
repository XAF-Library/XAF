﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WpfPlugin.ViewModels;
using XAF.Modularity.Abstraction;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.Modules;

namespace WpfPlugin;
public class WPFModule : UiModule
{

    public override async Task RegisterModuleAsync(IModuleRegistrationContext context, CancellationToken cancellation)
    {
        await base.RegisterModuleAsync(context, cancellation).ConfigureAwait(false);
        ConfigureOptions(context.GetServiceCollection(), context.GetConfiguration());
    }

    public void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
    {
        var coll = services;
        var conf = configuration;
    }

    public override void ComposeView(IViewService viewService)
    {
        viewService.AddViewAsync<ViewAViewModel>("PageViews").Wait();
        viewService.AddViewAsync<ViewBViewModel>("PageViews").Wait();
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        
    }
}
