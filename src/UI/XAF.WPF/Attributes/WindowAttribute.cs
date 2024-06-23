using System.Runtime.InteropServices;
using System.Windows;
using XAF.UI.Abstraction.Attributes;

namespace XAF.UI.WPF.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class WindowAttribute : BundleDecoratorAttribute
{
    public Type WindowType { get; }

    public WindowAttribute(Type windowType)
    {
        WindowType = windowType;
    }
}


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class WindowAttribute<TWindow> : WindowAttribute
    where TWindow : Window
{

    public WindowAttribute()
        : base(typeof(TWindow))
    {
    }
}
