using System.Reflection;
using System.Windows;

namespace XAF.WPF.UI;
public interface IViewLocator
{
    FrameworkElement GetViewFor<TViewModel>();

    FrameworkElement GetViewFor(Type vmType);

    Task DiscoverViewsAsync(Assembly assembly);
}
