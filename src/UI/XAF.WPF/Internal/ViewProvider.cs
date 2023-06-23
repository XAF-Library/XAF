using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Internal;
internal class ViewProvider : IViewProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IViewCollection _viewCollection;
    public IReadOnlyViewCollection ViewCollection => _viewCollection;

    public ViewProvider(IViewCollection viewCollection, IServiceProvider serviceProvider)
    {
        _viewCollection = viewCollection;
        _serviceProvider = serviceProvider;
    }

    public FrameworkElement GetViewWithViewModel(ViewDescriptor viewDescriptor)
    {
        var vm = (IViewModel)_serviceProvider.GetRequiredService(viewDescriptor.ViewModelType);

        FrameworkElement view = (FrameworkElement)_serviceProvider.GetRequiredService(viewDescriptor.ViewType);
        view.DataContext = vm;

        return view;
    }

    public FrameworkElement GetViewWithViewModel(IViewModel viewModel)
    {
        var descriptor = _viewCollection.GetDescriptorForViewModel(viewModel.GetType());

        var view = GetView(descriptor);
        view.DataContext = viewModel;
        return view;
    }

    public FrameworkElement GetView(ViewDescriptor viewDescriptor)
    {
        return (FrameworkElement)_serviceProvider.GetRequiredService(viewDescriptor.ViewType);
    }
}
