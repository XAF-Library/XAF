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
        _metadataByDecoratorType = new(); 
    }

    public void AddFromViewType(Type viewType)
    {
        var bundleDecorators = new BundleDecoratorCollection();
        bundleDecorators.AddFromType(viewType);

        if (!bundleDecorators.Contains<ViewForAttribute>())
        {
            return;
        }

        var vmType = bundleDecorators.GetDecoratorFirst<ViewForAttribute>().ViewModelType;
        var parameterType = vmType.IsGenericType ? vmType.GetGenericArguments()[0] : null;
        IBundleMetadata metadata = new BundleMetadata(vmType, viewType, bundleDecorators, parameterType);
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

    public IBundleMetadata GetMetadataForViewModel(Type viewModelType)
    {
        return _metadataByViewModelType[viewModelType];
    }
}

internal record BundleMetadata(Type ViewModelType, Type ViewType, IBundleDecoratorCollection ViewDecorators, Type? ParameterType) : IBundleMetadata;