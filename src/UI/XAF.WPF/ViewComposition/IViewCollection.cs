using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace XAF.UI.WPF.ViewComposition;

public interface IViewCollection : IReadOnlyViewCollection
{
    ViewDescriptor AddView(Type viewType);
    void AddLookupKey(ViewDescriptor viewDescriptor, object key);
}