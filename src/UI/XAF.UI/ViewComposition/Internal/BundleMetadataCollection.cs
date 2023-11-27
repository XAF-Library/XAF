using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.ViewComposition.Internal;
internal class BundleMetadataCollection : IBundleMetadataCollection
{
    private readonly Dictionary<Type, IBundleMetadata> _metadataByViewModelType;
    private readonly Dictionary<Type, List<IBundleMetadata>> _metadataByDecoratorType;

    public BundleMetadataCollection()
    {
        _metadataByViewModelType = new();
    }

    public void AddFromView(object view)
    {
        var viewTyp = view.GetType();
        var bundleDecorators = new BundleDecoratorCollection();
        bundleDecorators.AddFromType(viewTyp);

        if (!bundleDecorators.Contains<ViewForAttribute>())
        {
            return;
        }

        var vmType = bundleDecorators.GetDecorator<ViewForAttribute>().ViewModelType;
        IBundleMetadata metadata = new BundleMetadata(vmType, viewTyp, bundleDecorators);
        _metadataByViewModelType.Add(vmType, metadata);
        
        foreach (var decoratorType in bundleDecorators.GetAllDecoratorTypes())
        {
            _metadataByDecoratorType.Add(decoratorType, metadata);
        }
    }

    public IEnumerable<IBundleMetadata> GetMetadataForDecorator<TViewDecorator>() where TViewDecorator : BundleDecoratorAttribute
    {
        if (!_metadataByDecoratorType.ContainsKey(typeof(TViewDecorator)))
        {
            return Enumerable.Empty<IBundleMetadata>();
        }

        return _metadataByDecoratorType[typeof(TViewDecorator)];
    }

    public IBundleMetadata GetMetadataForViewModel<TViewModel>() where TViewModel : IXafViewModel
    {
        return _metadataByViewModelType[typeof(TViewModel)];
    }
}

internal record BundleMetadata(Type ViewModelType, Type ViewType, IBundleDecoratorCollection ViewDecorators) : IBundleMetadata;