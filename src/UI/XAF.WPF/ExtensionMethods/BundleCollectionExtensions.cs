using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.WPF.ExtensionMethods;
public static class BundleCollectionExtensions
{
    public static void AddFromAssembly(this IBundleMetadataCollection metadataCollection, Assembly assembly)
    {
        var types = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(FrameworkElement)))
            .ToArray();

        foreach (var type in types)
        {
            metadataCollection.AddFromViewType(type);
        }
    }
}
