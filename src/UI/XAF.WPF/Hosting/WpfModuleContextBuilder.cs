using System.CodeDom;
using XAF.Hosting.Abstraction;
using XAF.Modularity;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Hosting;

public class WpfModuleContextBuilder : ModuleRegistrationContextBuilder
{
    private readonly IViewCollection _viewCollection;
    private readonly IViewAdapterCollection _viewAdapterCollection;

    public WpfModuleContextBuilder(IViewCollection viewCollection, IViewAdapterCollection viewAdapterCollection)
    {
        _viewCollection = viewCollection;
        _viewAdapterCollection = viewAdapterCollection;
    }

    protected override void ProvideContextObjects(IXafHostBuilder builder)
    {
        base.ProvideContextObjects(builder);
        AddContextObject(_viewCollection);
        AddContextObject(_viewAdapterCollection);
    }
}