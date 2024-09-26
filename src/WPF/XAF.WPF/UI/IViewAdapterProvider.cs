using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace XAF.WPF.UI;
public interface IViewAdapterProvider
{

    bool TryGetAdapterFor<TView>([NotNullWhen(true)] out IViewAdapter? viewAdapter)
        where TView : FrameworkElement;

    bool TryGetAdapterFor(FrameworkElement element, [NotNullWhen(true)] out IViewAdapter? adapter);

}
