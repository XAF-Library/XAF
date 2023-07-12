using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using XAF.UI.WPF.Attributes;
using XAF.UI.WPF.ExtensionMethodes;

namespace XAF.UI.WPF.ViewComposition;

public class ViewDescriptor
{
    private readonly Dictionary<Type, object> _decoratorValues = new();

    public ViewDescriptor(Type viewType, Type viewModelType, IEnumerable<Attribute> viewAttributes)
    {
        ArgumentNullException.ThrowIfNull(viewType);
        ArgumentNullException.ThrowIfNull(viewModelType);

        ViewType = viewType;
        ViewModelType = viewModelType;
        ViewAttributes = viewAttributes;
        ViewAttributeTypes = viewAttributes.Select(a => a.GetType()).ToArray();
    }

    public Type ViewType { get; }

    public IEnumerable<Attribute> ViewAttributes { get; }

    public IEnumerable<Type> ViewAttributeTypes { get; }

    public Type ViewModelType { get; }

    public T GetDecoratorValue<TAttribute, T>()
        where TAttribute : Attribute
    {
        var value = _decoratorValues[typeof(TAttribute)];
        return value is T tValue
            ? tValue
            : throw new InvalidCastException($"value of decorator is of type: {value.GetType()}");
    }

    public bool TryGetDecoratorValue<TAttribute, T>([NotNullWhen(true)]out T? value)
        where TAttribute : Attribute
    {
        value = default;
        if(!_decoratorValues.TryGetValue(typeof(TAttribute), out var v))
        {
            return false;
        }

        if(value is not T tValue)
        {
            return false;
        }
        
        value = tValue;
        
        return true;
    }

    internal void AddDecoratorValue(Type attributeType, object value)
    {
        _decoratorValues[attributeType] = value;
    }

    public static ViewDescriptor Create(Type viewType)
    {
        ArgumentNullException.ThrowIfNull(viewType);

        if (viewType.IsAssignableFrom(typeof(FrameworkElement)))
        {
            throw new NotSupportedException($"view type must be an {typeof(FrameworkElement).FullName}");
        }
        var attributes = viewType.GetCustomAttributes().OfType<Attribute>().ToArray();

        var viewForAttribute = attributes.OfType<ViewForAttribute>().FirstOrDefault()
            ?? throw new NotSupportedException($"view must be annotated with an {typeof(ViewForAttribute<>).FullName}");

        var descriptor = new ViewDescriptor(viewType, viewForAttribute.ViewModelType, attributes);

        return descriptor;
    }
}