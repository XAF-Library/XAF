using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XAF.UI.Abstraction;

namespace XAF.UI.WPF.ViewComposition;
public interface IViewProvider
{
    IReadOnlyViewCollection ViewCollection { get; }
    FrameworkElement GetViewWithViewModel(ViewDescriptor viewDescriptor);

    FrameworkElement GetViewWithViewModel(IViewModel viewModel);

    FrameworkElement GetView(ViewDescriptor viewDescriptor);

}
