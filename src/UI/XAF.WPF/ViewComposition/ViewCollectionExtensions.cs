using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using XAF.UI.WPF.Attributes;
using XAF.UI.WPF.Internal;

namespace XAF.UI.WPF.ViewComposition;
public static class ViewCollectionExtensions
{
    public static IViewDescriptorCollection AddViewsFromAssembly(this IViewDescriptorCollection viewCollection, Assembly assembly)
    {
        var possibleTypes = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(FrameworkElement)) && t.GetCustomAttribute(typeof(ViewForAttribute)) != null);

        foreach (var type in possibleTypes)
        {
            viewCollection.AddView(type);
        }

        return viewCollection;
    }
}
