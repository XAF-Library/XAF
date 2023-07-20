using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Internal;
internal class ViewProvider : IViewProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IViewDescriptorProvider _viewCollection;
    public IViewDescriptorProvider ViewDescriptorProvider => _viewCollection;

    public ViewProvider(IViewDescriptorProvider viewCollection, IServiceProvider serviceProvider)
    {
        _viewCollection = viewCollection;
        _serviceProvider = serviceProvider;
    }

    public (FrameworkElement, IViewModel) GetViewWithViewModel(ViewDescriptor viewDescriptor)
    {
        var vm = (IViewModel)_serviceProvider.GetRequiredService(viewDescriptor.ViewModelType);

        FrameworkElement view = (FrameworkElement)_serviceProvider.GetRequiredService(viewDescriptor.ViewType);
        view.DataContext = vm;

        return (view,vm);
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
