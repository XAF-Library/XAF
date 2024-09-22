using System.ComponentModel;
using System.Linq.Expressions;

namespace XAF.Core.ExtensionMethods;

/// <summary>
/// Several extensions to the <see cref="INotifyPropertyChanged"/>
/// </summary>
public static class INotifyPropertyChangedExtensions
{

    /// <summary>
    /// Subscribe to an <see cref="PropertyChangedEventHandler"/> for the specified property
    /// </summary>
    /// <typeparam name="T">The class type which implements <see cref="INotifyPropertyChanged"/></typeparam>
    /// <typeparam name="TProperty">the type of the property</typeparam>
    /// <param name="notifier">The class which implements <see cref="INotifyPropertyChanged"/></param>
    /// <param name="propertyExpression">the expression to select the property</param>
    /// <param name="callback">the callback which is called when the property changes its value</param>
    public static void Subscribe<T, TProperty>(this T notifier, Expression<Func<T, TProperty>> propertyExpression, Action<TProperty> callback)
        where T : INotifyPropertyChanged
    {
        var name = propertyExpression.GetPropertyName();
        var reader = propertyExpression.Compile();
        notifier.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == name)
            {
                callback(reader(notifier));
            }
        };
    }
}
