using System.Diagnostics.CodeAnalysis;

namespace XAF.UI.WPF.ViewComposition;

public interface IViewDescriptorProvider
{
    ViewDescriptor GetDescriptorForViewModel(Type viewModelType);
    bool TryGetDescriptorForViewModel(Type viewModelType, [MaybeNullWhen(false)] out ViewDescriptor descriptor);
    IEnumerable<ViewDescriptor> GetDescriptorsByDecorator<TAttribute>()
        where TAttribute : Attribute;

    bool ContainsDescriptorWithDecorator<TAttribute>()
        where TAttribute : Attribute;
}