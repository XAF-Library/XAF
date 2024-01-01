using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.WPF.ExtensionMethods;
public static class ViewAdapterCollectionExtensioins
{
    public static void AddAdaptersFromAssembly(this IViewAdapterCollection adapterCollection, Assembly assembly)
    {
        var types = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IViewAdapter)));
        
        foreach (var adapterType in types)
        {
            adapterCollection.AddAdapter(adapterType);
        }
    }
}
