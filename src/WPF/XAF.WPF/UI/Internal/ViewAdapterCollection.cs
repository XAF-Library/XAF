using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using XAF.WPF.UI.ViewAdapters;

namespace XAF.WPF.UI.Internal;
internal class ViewAdapterCollection : IViewAdapterCollection
{
    private readonly ConcurrentDictionary<Type, IViewAdapter> _viewAdaptersByViewType;
    public void AddAdapter(IViewAdapter adapter)
    {
        _viewAdaptersByViewType.TryAdd(adapter.ContainerType, adapter);
    }

    public bool TryGetAdapterFor<TView>([NotNullWhen(true)] out IViewAdapter? viewAdapter) where TView : FrameworkElement
    {
        return TryGetAdapterFor(typeof(TView), out viewAdapter);
    }

    public bool TryGetAdapterFor(FrameworkElement element, [NotNullWhen(true)] out IViewAdapter? adapter)
    {
        return TryGetAdapterFor(element.GetType(), out adapter);
    }

    private bool TryGetAdapterFor(Type containerType, [NotNullWhen(true)] out IViewAdapter? adapter)
    {
        var type = containerType;

        while (type != null)
        {
            if (_viewAdaptersByViewType.ContainsKey(type))
            {
                return _viewAdaptersByViewType.TryGetValue(type, out adapter);
            }

            type = type.BaseType;
        }

        adapter = null;
        return false;
    }
}
