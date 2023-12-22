using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace XAF.UI.WPF.ExtensionMethods;
public static class DependencyObjectExtensions
{
    public static bool HasBinding(this FrameworkElement element, DependencyProperty property)
    {
        return BindingOperations.GetBinding(element, property) != null;
    }
}
