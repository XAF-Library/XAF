using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Windows;
using XAF.WPF.UI.Attributes;

namespace XAF.WPF.UI.Internal;
internal class DefaultViewLocator : IViewLocator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<(Type ViewModelType, PresentationType PresentationType), Type> _viewTypeByViewModelType;

    public DefaultViewLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _viewTypeByViewModelType = [];
    }

    public Task DiscoverViewsAsync(Assembly assembly)
    {
        var viewTypes = assembly.GetExportedTypes()
            .Where(t => t.IsAssignableTo(typeof(FrameworkElement)));

        foreach (var viewType in viewTypes)
        {
            foreach (var attribute in viewType.GetCustomAttributes<ViewForAttribute>())
            {
                _viewTypeByViewModelType.Add((attribute.ViewModelType, attribute.PresentationType), viewType);
            }
        }

        return Task.CompletedTask;
    }

    public FrameworkElement GetViewFor<TViewModel>()
    {
        return GetViewFor<TViewModel>(PresentationType.Both);
    }

    public FrameworkElement GetViewFor<TViewModel>(PresentationType presentationType)
    {
        return GetViewFor(typeof(TViewModel), presentationType);
    }

    public FrameworkElement GetViewFor(Type vmType)
    {
        return GetViewFor(vmType, PresentationType.Both);
    }

    public FrameworkElement GetViewFor(Type vmType, PresentationType presentationType)
    {
        if (!_viewTypeByViewModelType.TryGetValue((vmType, presentationType), out var viewType))
        {
            throw new Exception($"could not find a view for {vmType.FullName} with presentationType {presentationType}");
        }

        return (FrameworkElement)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, viewType);
    }
}
