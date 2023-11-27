using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition.Internal;
internal class ViewAdapterCollection : IViewAdapterCollection
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<Type, IViewAdapter> _adaptersByViewType;

    public ViewAdapterCollection(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void AddAdapter<TViewAdapter>() where TViewAdapter : IViewAdapter
    {
        var adapter = (IViewAdapter)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(TViewAdapter));
        _adaptersByViewType.Add(adapter.ViewType, adapter);
    }

    public IViewAdapter GetAdapterFor(Type viewType)
    {
        var type = viewType;
        while (type != null)
        {
            if (_adaptersByViewType.TryGetValue(type, out var adapter))
            {
                return adapter;
            }

            type = type.BaseType;
        }

        throw new KeyNotFoundException($"No adapter for view type {viewType.FullName}");
    }
}
