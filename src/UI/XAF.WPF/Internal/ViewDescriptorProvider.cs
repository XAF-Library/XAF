using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ViewComposition;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.Internal;
internal class ViewDescriptorProvider : IViewDescriptorProvider
{
    private readonly Dictionary<Type, ViewDescriptor> _viewDescriptorsByVmType;
    private readonly Dictionary<Type, HashSet<ViewDescriptor>> _viewDescriptorsByDecorators;

    public ViewDescriptorProvider(Dictionary<Type, ViewDescriptor> viewDescriptorsByVmType, Dictionary<Type, HashSet<ViewDescriptor>> viewDescriptorLookup)
    {
        _viewDescriptorsByVmType = viewDescriptorsByVmType;
        _viewDescriptorsByDecorators = viewDescriptorLookup;
    }

    public ViewDescriptor GetDescriptorForViewModel(Type viewModelType)
    {
        return _viewDescriptorsByVmType[viewModelType];
    }

    public bool TryGetDescriptorForViewModel(Type viewModelType, [MaybeNullWhen(false)] out ViewDescriptor descriptor)
    {
        return _viewDescriptorsByVmType.TryGetValue(viewModelType, out descriptor);
    }

    public IEnumerable<ViewDescriptor> GetDescriptorsByDecorator<TAttribute>() where TAttribute : Attribute
    {
        
        return _viewDescriptorsByDecorators.TryGetValue(typeof(TAttribute), out var values)
            ? values
            : Enumerable.Empty<ViewDescriptor>();
    }

    public bool ContainsDescriptorWithDecorator<TAttribute>() where TAttribute : Attribute
    {
        return _viewDescriptorsByDecorators.ContainsKey(typeof(TAttribute));
    }
}
