using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using XAF.Modularity;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ExtensionMethods;

namespace XAF.UI.WPF.Modules;
public abstract class WpfModuleStartUp : IModuleStartup
{
    private readonly IBundleMetadataCollection _metadataCollection;
    private readonly IViewAdapterCollection _adapterCollection;
    private readonly IViewService _viewService;

    public WpfModuleStartUp(
        IBundleMetadataCollection metadataCollection,
        IViewAdapterCollection adapterCollection,
        IViewService viewService)
    {
        _metadataCollection = metadataCollection;
        _adapterCollection = adapterCollection;
        _viewService = viewService;
    }

    public virtual async Task StartAsync(CancellationToken cancellationToken)
    {
        RegisterViews(_metadataCollection);
        RegisterAdapters(_adapterCollection);
        await ComposeViewsAsync(_viewService).ConfigureAwait(false);
    }

    protected virtual void RegisterViews(IBundleMetadataCollection metadataCollection)
    {
        metadataCollection.AddFromAssembly(Assembly.GetAssembly(this.GetType()));
    }

    protected virtual void RegisterAdapters(IViewAdapterCollection adapterCollection)
    {
        adapterCollection.AddAdaptersFromAssembly(Assembly.GetAssembly(this.GetType()));
    }

    protected virtual Task ComposeViewsAsync(IViewService viewService)
    {
        return Task.CompletedTask;
    }
}
