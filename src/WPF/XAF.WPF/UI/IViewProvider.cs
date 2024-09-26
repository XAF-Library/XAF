using System.Windows;

namespace XAF.WPF.UI;
public interface IViewProvider
{
    ViewProviderOptions Options { get; }

    FrameworkElement GetViewFor<TViewModel>();

    FrameworkElement GetViewFor<TViewModel>(Dictionary<object, object> metaData);

    FrameworkElement GetViewFor(Type vmType);
    FrameworkElement GetViewFor(Type vmType, Dictionary<object, object> metaData);
}
