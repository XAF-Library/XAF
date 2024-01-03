using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XAF.Modularity;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ExtensionMethods;

namespace XAF.UI.WPF.Modules.Internal;
internal class DefaultWpfStartup : IModuleStartup
{
    private readonly Assembly _assembly;
    private readonly IBundleMetadataCollection _metadataCollection;
    private readonly IViewAdapterCollection _viewAdapterCollection;

    public DefaultWpfStartup(
        Assembly assembly,
        IBundleMetadataCollection metadataCollection,
        IViewAdapterCollection viewAdapterCollection)
    {
        _assembly = assembly;
        _metadataCollection = metadataCollection;
        _viewAdapterCollection = viewAdapterCollection;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _metadataCollection.AddFromAssembly(_assembly);
        _viewAdapterCollection.AddAdaptersFromAssembly(_assembly);
        return Task.CompletedTask;
    }
}
