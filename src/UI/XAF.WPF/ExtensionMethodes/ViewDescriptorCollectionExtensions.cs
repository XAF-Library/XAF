using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI;
using XAF.UI.WPF.Attributes;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.ExtensionMethodes;
public static class ViewDescriptorCollectionExtensions
{
    public static void AddDefaultDecorators(this IViewDescriptorCollection collection)
    {
        collection.AddDecorator<DialogWindowAttribute, Type>(a => a.WindowType);

        collection.AddDecorator<ContainsViewContainerAttribute, object>(a =>
        {
            var keys = new HashSet<object>();
            foreach (var attribute in a)
            {
                keys.Add(attribute.Key);
            }

            return keys;
        });
    }

    public static void AddDefaultInitilizers(this IViewDescriptorCollection collection)
    {
        collection.AddDescriptorInitilizer<DialogWindowAttribute>(
            (a, d, s) =>
            {
                s.AddTransient(a.WindowType);
            });
    }
}
