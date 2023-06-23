﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WpfPlugin.ViewModels;
using XAF;
using XAF.Modularity;
using XAF.Modularity.Abstraction;
using XAF.WPF.Modules;

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

    public override void ComposeView(IViewCompositionService viewCompositionService)
    {
        viewCompositionService.InsertView<ViewAViewModel>("PageViews");
        viewCompositionService.InsertView(typeof(ViewBViewModel), "PageViews");
    }

    protected override void RegisterServices(IServiceCollection services)
    {
        
    }
}
