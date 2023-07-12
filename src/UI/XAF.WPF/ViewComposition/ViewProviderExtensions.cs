using System.Runtime.CompilerServices;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.WPF.Attributes;
using XAF.UI.WPF.Internal;

namespace XAF.UI.WPF.ViewComposition;
public static class ViewProviderExtensions
{
    public static (FrameworkElement, TViewModel) GetViewWithViewModel<TViewModel>(this IViewProvider viewProvider)
        where TViewModel : IViewModel
    {
        var (view, vm) = viewProvider.GetViewWithViewModel(typeof(TViewModel));
        return (view, (TViewModel)vm);
    }

    public static Window GetShell(this IViewProvider viewProvider)
    {
        var descriptor = viewProvider.ViewDescriptorProvider.GetDescriptorsByDecorator<ShellAttribute>().FirstOrDefault()
            ?? throw new InvalidOperationException("no shell registered");

        var (view, _) = viewProvider.GetViewWithViewModel(descriptor);

        return (Window)view;
    }

    public static ViewDescriptor GetViewDescriptorForViewModel(this IViewProvider viewProvider, Type vmType)
    {
        return viewProvider.ViewDescriptorProvider.GetDescriptorForViewModel(vmType);
    }

    public static (FrameworkElement, IViewModel) GetViewWithViewModel(this IViewProvider viewProvider, Type viewModelType)
    {
        return !viewProvider.ViewDescriptorProvider.TryGetDescriptorForViewModel(viewModelType, out var viewDescriptor)
            ? throw new KeyNotFoundException($"no view for view model type \"{viewModelType.FullName}\" found")
            : viewProvider.GetViewWithViewModel(viewDescriptor);
    }

    public static FrameworkElement GetView(this IViewProvider viewProvider, Type viewModelType)
    {
        var descriptor = viewProvider.GetViewDescriptorForViewModel(viewModelType);

        return viewProvider.GetView(descriptor);
    }
}
