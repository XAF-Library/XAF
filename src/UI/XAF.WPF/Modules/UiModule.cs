using Microsoft.Extensions.DependencyInjection;
using XAF.Modularity.Abstraction;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ExtensionMethods;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ExtensionMethods;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Modules;
public abstract class UiModule : Module
{

    public override Task StartModuleAsync(IServiceProvider services, CancellationToken cancellation)
    {
        RegisterViews(services.GetRequiredService<IBundleMetadataCollection>());
        ComposeView(services.GetRequiredService<IViewService>());
        return Task.CompletedTask;
    }

    public override async Task RegisterModuleAsync(IModuleRegistrationContext context, CancellationToken cancellation)
    {
        await base.RegisterModuleAsync(context, cancellation).ConfigureAwait(false);
    }

    public virtual void RegisterViews(IBundleMetadataCollection metadataCollection)
    {
        metadataCollection.AddFromAssembly(GetType().Assembly);
    }

    public virtual void ComposeView(IViewService viewService) { }

}
