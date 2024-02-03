using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.ViewComposition.Internal;
internal class BundleDecoratorCollection : IBundleDecoratorCollection
{
    private readonly Dictionary<Type, List<BundleDecoratorAttribute>> _decorators;

    public bool Contains<TViewDecorator>() where TViewDecorator : BundleDecoratorAttribute
    {
        return _decorators.ContainsKey(typeof(TViewDecorator));
    }

    public IEnumerable<TViewDecorator> GetDecorators<TViewDecorator>() where TViewDecorator : BundleDecoratorAttribute
    {
        if (!_decorators.ContainsKey(typeof(TViewDecorator)))
        {
            return Enumerable.Empty<TViewDecorator>();
        }

        return _decorators[typeof(TViewDecorator)].OfType<TViewDecorator>();
    }

    public TViewDecorator GetFirstDecorator<TViewDecorator>() where TViewDecorator : BundleDecoratorAttribute
    {
        return (TViewDecorator)_decorators[typeof(TViewDecorator)].First();
    }

    public bool TryGetDecorator<TViewDecorator>([NotNullWhen(true)] out TViewDecorator? decorator) where TViewDecorator : BundleDecoratorAttribute
    {
        decorator = null;

        if (!_decorators.ContainsKey(typeof(TViewDecorator)))
        {
            return false;
        }

        decorator = _decorators[typeof(TViewDecorator)].FirstOrDefault() as TViewDecorator;

        return decorator is not null;
    }

    public BundleDecoratorCollection()
    {
        _decorators = new();
    }

    public void Add(BundleDecoratorAttribute decoratorAttribute)
    {
        var type = decoratorAttribute.GetType();

        while (type != null && type != typeof(BundleDecoratorAttribute))
        {
            _decorators.Add(type, decoratorAttribute);
            type = type.BaseType;
        }
    }

    public void AddFromType(Type type)
    {
        var attributes = type.GetCustomAttributes<BundleDecoratorAttribute>();
        foreach (var attribute in attributes)
        {
            Add(attribute);
        }

    }

    public IEnumerable<BundleDecoratorAttribute> GetAllDecorators()
    {
        return _decorators.Values.SelectMany(v => v);
    }

    public IEnumerable<Type> GetAllDecoratorTypes()
    {
        return _decorators.Keys;
    }
}
