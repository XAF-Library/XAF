using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.WPF.ViewComposition;
public interface IViewProvider
{
    IViewDescriptorProvider ViewDescriptorProvider { get; }

    (FrameworkElement, IViewModel) GetViewWithViewModel(ViewDescriptor viewDescriptor);

    FrameworkElement GetViewWithViewModel(IViewModel viewModel);

    FrameworkElement GetView(ViewDescriptor viewDescriptor);

}
