using System.Runtime.CompilerServices;
using System.Windows;
using XAF.UI.Abstraction;
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
        var descriptor = viewProvider.ViewCollection.GetDescriptorsByKey(ViewDescriptorKeys.IsShellKey).FirstOrDefault()
            ?? throw new InvalidOperationException("no shell registered");

        var (view, _) = viewProvider.GetViewWithViewModel(descriptor);

        return (Window)view;
    }

    public static bool IsSplashScreenRegistered(this IViewProvider viewProvider)
    {
        return viewProvider.ViewCollection.GetDescriptorsByKey(ViewDescriptorKeys.IsSplashScreenKey).Any();
    }

    public static Window GetSplashScreen(this IViewProvider viewProvider)
    {
        var descriptor = viewProvider.ViewCollection.GetDescriptorsByKey(ViewDescriptorKeys.IsSplashScreenKey).FirstOrDefault()
            ?? throw new InvalidOperationException("no splash screen registered");
        var (view, _) = viewProvider.GetViewWithViewModel(descriptor);
        return (Window)view;
    }

    public static ViewDescriptor GetViewDescriptorForViewModel(this IViewProvider viewProvider, Type vmType)
    {
        return viewProvider.ViewCollection.GetDescriptorForViewModel(vmType);
    }

    public static bool IsNavigableView(this IViewProvider viewProvider, Type vmType)
    {
        var descriptor = viewProvider.GetViewDescriptorForViewModel(vmType);

        return descriptor?.LookupKeys.Contains(ViewDescriptorKeys.IsNavigableKey) ?? false;
    }

    public static (FrameworkElement, IViewModel) GetViewWithViewModel(this IViewProvider viewProvider, Type viewModelType)
    {
        return !viewProvider.ViewCollection.TryGetDescriptorForViewModel(viewModelType, out var viewDescriptor)
            ? throw new KeyNotFoundException($"no view for view model type \"{viewModelType.FullName}\" found")
            : viewProvider.GetViewWithViewModel(viewDescriptor);
    }

    public static FrameworkElement GetView(this IViewProvider viewProvider, Type viewModelType)
    {
        var descriptor = viewProvider.GetViewDescriptorForViewModel(viewModelType);

        return viewProvider.GetView(descriptor);
    }
}
