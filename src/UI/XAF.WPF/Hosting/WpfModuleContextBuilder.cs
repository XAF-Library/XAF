using System.CodeDom;
using XAF.Hosting.Abstraction;
using XAF.Modularity;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Hosting;

public class WpfModuleContextBuilder : ModuleRegistrationContextBuilder
{
    private readonly IBundleMetadataCollection _viewCollection;
    private readonly IViewAdapterCollection _viewAdapterCollection;

    public WpfModuleContextBuilder(IBundleMetadataCollection metadataCollection, IViewAdapterCollection viewAdapterCollection)
    {
        _viewCollection = metadataCollection;
        _viewAdapterCollection = viewAdapterCollection;
    }

    protected override void ProvideContextObjects(IXafHostBuilder builder)
    {
        base.ProvideContextObjects(builder);
        AddContextObject(_viewCollection);
        AddContextObject(_viewAdapterCollection);
    }
}