using System.Runtime.InteropServices;
using System.Windows;

namespace XAF.UI.WPF.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DialogWindowAttribute : Attribute
{
    public Type WindowType { get; }

    public DialogWindowAttribute(Type windowType)
    {
        WindowType = windowType;
    }
}


[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DialogWindowAttribute<TWindow> : DialogWindowAttribute
    where TWindow : Window
{

    public DialogWindowAttribute()
        : base(typeof(TWindow))
    {
    }
}
