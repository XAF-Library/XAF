using System.Diagnostics.CodeAnalysis;
using XAF.UI.Abstraction;

namespace XAF.UI.Abstraction.ViewComposition;

public interface IViewDescriptorProvider
{
    ViewDescriptor GetDescriptorForViewModel(Type viewModelType);
    bool TryGetDescriptorForViewModel(Type viewModelType, [MaybeNullWhen(false)] out ViewDescriptor descriptor);
    IEnumerable<ViewDescriptor> GetDescriptorsByDecorator<TAttribute>()
        where TAttribute : Attribute;

    bool ContainsDescriptorWithDecorator<TAttribute>()
        where TAttribute : Attribute;
}