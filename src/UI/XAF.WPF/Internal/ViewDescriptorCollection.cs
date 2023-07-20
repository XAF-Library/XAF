using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.Attributes;
using XAF.UI.WPF.ViewComposition;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.Internal;
internal class ViewDescriptorCollection : IViewDescriptorCollection
{
    private readonly List<ViewDescriptor> _viewDescriptors = new();
    private readonly Dictionary<object, HashSet<ViewDescriptor>> _lookupDictionary = new();
    private readonly Dictionary<Type, ViewDescriptor> _vmDictionary = new();
    private readonly IServiceCollection _services;
    private readonly Dictionary<Type, Func<Attribute, object>> _singleAttributeDecorators = new();
    private readonly Dictionary<Type, Func<IEnumerable<Attribute>, object>> _multipleAttributeDecorators = new();
    private readonly Dictionary<Type, Action<Attribute, ViewDescriptor, IServiceCollection>> _initilizers = new();
    private bool _builded;

    public int Count => _viewDescriptors.Count;
    public bool IsReadOnly => false;

    public ViewDescriptor this[Type key] => _vmDictionary[key];

    public IEnumerable<ViewDescriptor> this[object key] => _lookupDictionary.TryGetValue(key, out var descriptors) ? descriptors : Enumerable.Empty<ViewDescriptor>();

    public ViewDescriptorCollection(IServiceCollection services)
    {
        _services = services;
    }

    public ViewDescriptor AddView(Type viewType)
    {
        if (_builded)
        {
            throw new InvalidOperationException("Can't add new view to collection. ViewProvider allready builded");
        }
        var descriptor = ViewDescriptor.Create(viewType);

        _services.AddTransient(descriptor.ViewType);
        _services.AddTransient(descriptor.ViewModelType);

        foreach (var attribute in descriptor.ViewAttributes)
        {
            var type = attribute.GetType();
            if(type.IsGenericType && type.BaseType != null)
            {
                type = type.BaseType;
            }

            if (!_initilizers.TryGetValue(type, out var initilizer))
            {
                continue;
            }
            initilizer(attribute, descriptor, _services);
        }

        _viewDescriptors.Add(descriptor);
        return descriptor;
    }

    public IViewDescriptorProvider BuildViewDescriptorProvider()
    {
        if (_builded)
        {
            throw new InvalidOperationException("View description provider allready builded");
        }

        var viewDescriptorsByVmType = new Dictionary<Type, ViewDescriptor>();
        var viewDescriptorsByDecorators = new Dictionary<Type, HashSet<ViewDescriptor>>();

        foreach (var descriptor in _viewDescriptors)
        {
            foreach (var attribute in descriptor.ViewAttributes.GroupBy(a => a.GetType()))
            {
                var type = attribute.Key;
                if (type.IsGenericType && type.BaseType != null)
                {
                    type = type.BaseType;
                }

                if (_multipleAttributeDecorators.TryGetValue(type, out var multiDecorator))
                {
                    var value = multiDecorator.Invoke(attribute);
                    descriptor.AddDecoratorValue(attribute.Key, value);
                    continue;
                }

                if (_singleAttributeDecorators.TryGetValue(type, out var singleDecorator))
                {
                    var value = singleDecorator.Invoke(attribute.First());
                    descriptor.AddDecoratorValue(attribute.Key, value);
                }
            }
            viewDescriptorsByDecorators.Add(descriptor.ViewAttributeTypes, descriptor);
            viewDescriptorsByVmType.Add(descriptor.ViewModelType, descriptor);
        }

        var provider = new ViewDescriptorProvider(viewDescriptorsByVmType, viewDescriptorsByDecorators);
        _builded = true;
        return provider;
    }

    public void AddDecorator<TAttribute, TDecorator>(Func<TAttribute, TDecorator> action) where TAttribute : Attribute
    {
        _singleAttributeDecorators.Add(typeof(TAttribute), a => action.Invoke((TAttribute)a));
    }

    public void AddDecorator<TAttribute, TDecorator>(Func<IEnumerable<TAttribute>, TDecorator> action) where TAttribute : Attribute
    {
        _multipleAttributeDecorators.Add(typeof(TAttribute), a => action.Invoke(a.OfType<TAttribute>()));
    }

    public void AddDescriptorInitilizer<TAttribute>(Action<TAttribute, ViewDescriptor, IServiceCollection> action)
        where TAttribute : Attribute
    {
        _initilizers.Add(typeof(TAttribute), (a, d, s) => action((TAttribute)a, d, s));
    }
}
