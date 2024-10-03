using System.Windows;

namespace XAF.WPF.UI;
public interface IViewProvider
{
    ViewProviderOptions Options { get; }

    FrameworkElement GetViewFor<TViewModel>();

    FrameworkElement GetViewFor(Type vmType);

}
