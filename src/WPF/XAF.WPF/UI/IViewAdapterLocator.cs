using System.Diagnostics.CodeAnalysis;
using System.Windows;
using XAF.WPF.UI.ViewAdapters;

namespace XAF.WPF.UI;
public interface IViewAdapterLocator
{
    bool TryGetAdapterFor<TView>([NotNullWhen(true)] out IViewAdapter? viewAdapter)
        where TView : FrameworkElement;

    bool TryGetAdapterFor(FrameworkElement element, [NotNullWhen(true)] out IViewAdapter? adapter);

    void AddAdapter(IViewAdapter adapter);
}
