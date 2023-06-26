﻿using Microsoft.Extensions.DependencyInjection;
using XAF.Modularity;
using XAF.Modularity.Abstraction;
using XAF.UI.Abstraction;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Modules;
public abstract class UiModule : Module
{

    public override Task StartModuleAsync(IServiceProvider services, CancellationToken cancellation)
    {
        ComposeView(services.GetRequiredService<IViewCompositionService>());
        return Task.CompletedTask;
    }

    public override async Task RegisterModuleAsync(IModuleRegistrationContext context, CancellationToken cancellation)
    {
        await base.RegisterModuleAsync(context, cancellation).ConfigureAwait(false);
        RegisterViews(context.Get<IViewCollection>());
    }

    public virtual void RegisterViews(IViewCollection viewCollection)
    {
        viewCollection.AddViewsFromAssembly(GetType().Assembly);
    }

    public abstract void ComposeView(IViewCompositionService viewCompositionService);

}
