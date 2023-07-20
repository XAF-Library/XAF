using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.WPF.Behaviors;
using XAF.UI.WPF.ViewComposition;
using XAF.UI.WPF.ExtensionMethodes;
using XAF.Utilities;
using XAF.Utilities.ExtensionMethods;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.WPF.Internal;
internal class NavigationService : INavigationService
{
    private readonly IViewProvider _viewProvider;
    private readonly IViewDescriptorProvider _viewDescriptorProvider;
    private readonly IViewAdapterCollection _viewAdapters;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<object, List<Action<ViewDescriptor, IViewModel>>> _navigationCallbacks = new();
    private readonly Dictionary<object, NavigationBackStack> _backStacks = new();
    private readonly Dictionary<object, List<FrameworkElement>> _viewCache = new();
    private readonly Dictionary<object, IViewAdapter> _viewAdapterForContainers = new();
    private readonly HashSet<object> _aviableNavKeys = new HashSet<object>();
    private uint _backStackCapacity = 1;

    public NavigationService(IViewProvider viewProvider,
        IViewDescriptorProvider viewDescriptorProvider,
        IViewAdapterCollection viewAdapters,
        IServiceProvider serviceProvider)
    {
        _viewProvider = viewProvider;
        _viewDescriptorProvider = viewDescriptorProvider;
        _viewAdapters = viewAdapters;
        _serviceProvider = serviceProvider;

        var navKeys = viewDescriptorProvider.GetDescriptorsByDecorator<ContainsViewContainerAttribute>()
            .SelectMany(d => d.GetNavigationKeys());

        foreach (var navKey in navKeys)
        {
            _aviableNavKeys.Add(navKey);
        }

    }

    public uint BackStackCapacity
    {
        get => _backStackCapacity;
        set
        {
            _backStackCapacity = value;
            foreach (var backStack in _backStacks.Values)
            {
                backStack.Capacity = _backStackCapacity;
            }
        }
    }

    public bool CanNavigateBack(object containerKey)
    {
        if (!_backStacks.TryGetValue(containerKey, out var backStack))
        {
            backStack = new(_backStackCapacity);
            _backStacks[containerKey] = backStack;
        }

        return backStack.CanNavigateBack;
    }

    public bool CanNavigateForward(object containerKey)
    {
        if (!_backStacks.TryGetValue(containerKey, out var backStack))
        {
            backStack = new(_backStackCapacity);
            _backStacks[containerKey] = backStack;
        }

        return backStack.CanNavigateForward;
    }

    public void RegisterCanNavigateBackChangedCallback(object containerKey, Action<bool> callback)
    {
        if (!_backStacks.TryGetValue(containerKey, out var backStack))
        {
            backStack = new(_backStackCapacity);
            _backStacks[containerKey] = backStack;
        }

        backStack.AddValueChangedCallBack(b => b.CanNavigateBack, callback);
    }

    public void ReigsterCanNavigateForwardChangedCallback(object containerKey, Action<bool> callback)
    {
        if (!_backStacks.TryGetValue(containerKey, out var backStack))
        {
            backStack = new(_backStackCapacity);
            _backStacks[containerKey] = backStack;
        }

        backStack.AddValueChangedCallBack(b => b.CanNavigateBack, callback);
    }

    public void NavigateBack(object containerKey)
    {
        if (!CanNavigateBack(containerKey))
        {
            throw new InvalidOperationException("can't navigate back");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var entry = _backStacks[containerKey].NavigateBack();
            var adapter = _viewAdapterForContainers[containerKey];

            var oldView = adapter.GetActiveView(container)!;

            if (oldView.DataContext is INavigationTarget oldVm)
            {
                oldVm.OnNavigatedFrom();
            }

            adapter.Select(container, entry.View);

            if (entry.View.DataContext is INavigationTarget newVm)
            {
                newVm.OnNavigatedTo();
            }
        });
    }

    public void NavigateForward(object containerKey)
    {
        if (!CanNavigateForward(containerKey))
        {
            throw new InvalidOperationException("can't navigate forward");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var entry = _backStacks[containerKey].NavigateForward();
            var adapter = _viewAdapterForContainers[containerKey];

            var oldView = adapter.GetActiveView(container)!;

            if (oldView.DataContext is INavigationTarget oldVm)
            {
                oldVm.OnNavigatedFrom();
            }

            adapter.Select(container, entry.View);

            if (entry.View.DataContext is INavigationTarget newVm)
            {
                newVm.OnNavigatedTo();
            }
        });
    }

    public void NavigateTo<TViewModel>(object containerKey) where TViewModel : INavigationTarget
    {
        NavigateTo(typeof(TViewModel), containerKey);
    }

    public void NavigateTo<TViewModel, TParameter>(object containerKey, TParameter parameter) where TViewModel : INavigationTarget<TParameter>
    {

        if (!_aviableNavKeys.Contains(containerKey))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var newView = ExecuteNavigation(containerKey, container, typeof(TViewModel));

            if (newView.DataContext is INavigationTarget<TParameter> paramVm)
            {
                paramVm.OnNavigatedTo(parameter);
            }
            else if (newView.DataContext is INavigationTarget vm)
            {
                vm.OnNavigatedTo(parameter);
            }
        });
    }

    public void NavigateTo(Type viewModelType, object containerKey, object? parameter)
    {
        if (!_aviableNavKeys.Contains(containerKey))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var newView = ExecuteNavigation(containerKey, container, viewModelType);

            if (newView.DataContext is INavigationTarget vm)
            {
                vm.OnNavigatedTo(parameter);
            }
        });
    }

    public void NavigateTo<TViewModel>(object containerKey, TViewModel viewModel) where TViewModel : INavigationTarget
    {
        if (!_aviableNavKeys.Contains(containerKey))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var newView = ExecuteNavigation(containerKey, container, viewModel.GetType(), viewModel);

            viewModel.OnNavigatedTo();
        });
    }

    public void NavigateTo(Type viewModelType, object containerKey)
    {
        if (!_aviableNavKeys.Contains(containerKey))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var newView = ExecuteNavigation(containerKey, container, viewModelType);

            if (newView.DataContext is INavigationTarget paramVm)
            {
                paramVm.OnNavigatedTo();
            }
        });
    }

    public void AddNavigationCallback(object containerKey, Action<ViewDescriptor, IViewModel> callback)
    {
        if (!_aviableNavKeys.Contains(containerKey))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        _navigationCallbacks.Add(containerKey, callback);
    }

    private FrameworkElement ExecuteNavigation(object containerKey, FrameworkElement container, Type viewModelType, IViewModel? viewModel = null)
    {
        if (!_viewAdapterForContainers.TryGetValue(containerKey, out var adapter))
        {
            adapter = _viewAdapters.GetAdapterFor(container.GetType());
            _viewAdapterForContainers[containerKey] = adapter;
        }


        if (!_viewCache.TryGetValue(containerKey, out var cachedViews))
        {
            cachedViews = new();
            _viewCache[containerKey] = cachedViews;
        }

        if (!_backStacks.TryGetValue(containerKey, out var backStack))
        {
            backStack = new(_backStackCapacity);
            _backStacks[containerKey] = backStack;
        }

        var oldView = adapter.GetActiveView(container);

        if (oldView != null && oldView.DataContext is INavigationTarget vm)
        {
            vm.OnNavigatedFrom();
        }

        var newView = adapter.GetElements(container).FirstOrDefault(e => e.DataContext.GetType() == viewModelType);

        newView ??= cachedViews.FirstOrDefault(e => e.DataContext.GetType() == viewModelType);

        if (newView == null)
        {
            newView = _viewProvider.GetView(viewModelType);
            cachedViews.Add(newView);
        }

        if (newView.DataContext == null || viewModel == null)
        {
            viewModel = (IViewModel)_serviceProvider.GetRequiredService(viewModelType);
            newView.DataContext = viewModel;
        }

        backStack.Insert(newView);
        adapter.Select(container, newView);

        if (_navigationCallbacks.TryGetValue(containerKey, out var callbacks))
        {
            var descriptor = _viewDescriptorProvider.GetDescriptorForViewModel(viewModelType);
            foreach (var callback in callbacks)
            {
                callback(descriptor, viewModel);
            }
        }

        return newView;
    }
}
