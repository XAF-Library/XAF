using System.Reactive.Linq;
using System.Windows;
using XAF.WPF.UI;

namespace XAF.WPF.ExtenionMethods;
public static class ViewCollectionExtensions
{
    public static IObservable<FrameworkElement> First(this IViewCollection viewCollection)
    {
        return viewCollection.WhenChanged()
            .Where(c => c.Count > 0)
            .Select(c => c.First());
    }

    public static IObservable<FrameworkElement> Last(this IViewCollection viewCollection)
    {
        return viewCollection.WhenChanged()
            .Where(c => c.Count > 0)
            .Select(c => c.Last());
    }
}
