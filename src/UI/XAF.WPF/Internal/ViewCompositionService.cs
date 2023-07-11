using XAF.UI.Abstraction;
using XAF.UI.WPF.Behaviors;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Internal;
internal class ViewCompositionService : IViewCompositionService
{
    private readonly IReadOnlyViewCollection _viewCollection;
    private readonly IViewAdapterCollection _viewAdapters;
    private readonly IViewProvider _viewProvider;

    public ViewCompositionService(IViewAdapterCollection viewAdapters, IViewProvider viewProvider)
    {
        _viewCollection = viewProvider.ViewCollection;
        _viewAdapters = viewAdapters;
        _viewProvider = viewProvider;
    }

    public void ClearViews(object containerKey)
    {
        if (!_viewCollection.GetDescriptorsByKey(ViewDescriptorKeys.ContainsViewContainerKey)
           .Any(v => v.Properties.TryGetValue(ViewDescriptorKeys.ContainsViewContainerKey, out var navKeys) && navKeys is HashSet<object> hasSet && hasSet.Contains(containerKey)))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var adapter = _viewAdapters.GetAdapterFor(container.GetType());
            adapter.Clear(container);
        });

    }

    public void InsertView<TViewModel>(object containerKey) where TViewModel : IViewModel
    {
        InsertView(typeof(TViewModel), containerKey);
    }

    public void InsertView(Type viewMdoel, object containerKey)
    {
        if (!_viewCollection.GetDescriptorsByKey(ViewDescriptorKeys.ContainsViewContainerKey)
           .Any(v => v.Properties.TryGetValue(ViewDescriptorKeys.ContainsViewContainerKey, out var navKeys) && navKeys is HashSet<object> hasSet && hasSet.Contains(containerKey)))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var adapter = _viewAdapters.GetAdapterFor(container.GetType());
            var (view, _) = _viewProvider.GetViewWithViewModel(viewMdoel);
            adapter.Insert(container, view);
        });
    }

    public void InsertView<TViewModel>(TViewModel viewModel, object containerKey) where TViewModel : IViewModel
    {
        if (!_viewCollection.GetDescriptorsByKey(ViewDescriptorKeys.ContainsViewContainerKey)
           .Any(v => v.Properties.TryGetValue(ViewDescriptorKeys.ContainsViewContainerKey, out var navKeys) && navKeys is HashSet<object> hasSet && hasSet.Contains(containerKey)))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        ViewContainer.ExecuteContainerAction(containerKey, container =>
        {
            var adapter = _viewAdapters.GetAdapterFor(container.GetType());
            var view = _viewProvider.GetViewWithViewModel(viewModel);
            adapter.Insert(container, view);
        });
    }

    public void RemoveView<TViewModel>(object containerKey) where TViewModel : IViewModel
    {
        RemoveView(typeof(TViewModel), containerKey);
    }

    public void RemoveView<T>(T viewModel, object containerKey) where T : IViewModel
    {
        if (!_viewCollection.GetDescriptorsByKey(ViewDescriptorKeys.ContainsViewContainerKey).Any(v => v.Properties[ViewDescriptorKeys.ContainsViewContainerKey].Equals(containerKey)))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        if (!ViewContainer.ViewContainers.TryGetValue(containerKey, out var container))
        {
            if (!ViewContainer.InitialInsertions.TryGetValue(containerKey, out var initialInsertions))
            {
                initialInsertions = new();
                ViewContainer.InitialInsertions[containerKey] = initialInsertions;
            }

            initialInsertions.Add(c =>
            {
                var adapter = _viewAdapters.GetAdapterFor(c.GetType());
                var view = adapter.GetElements(c).FirstOrDefault(e => e.DataContext.Equals(viewModel));
                if (view != null)
                {
                    adapter.Remove(c, view);
                }
            });

            return;
        }

        var adapter = _viewAdapters.GetAdapterFor(container.GetType());
        var view = adapter.GetElements(container).FirstOrDefault(e => e.DataContext.Equals(viewModel));
        if (view != null)
        {
            adapter.Remove(container, view);
        }
    }

    public void RemoveView(Type viewModel, object containerKey)
    {
        if (!_viewCollection.GetDescriptorsByKey(ViewDescriptorKeys.ContainsViewContainerKey).Any(v => v.Properties[ViewDescriptorKeys.ContainsViewContainerKey].Equals(containerKey)))
        {
            throw new InvalidOperationException($"No navigation frame with the key: {containerKey} found.");
        }

        if (!ViewContainer.ViewContainers.TryGetValue(containerKey, out var container))
        {
            if (!ViewContainer.InitialInsertions.TryGetValue(containerKey, out var initialInsertions))
            {
                initialInsertions = new();
                ViewContainer.InitialInsertions[containerKey] = initialInsertions;
            }

            initialInsertions.Add(c =>
            {
                var adapter = _viewAdapters.GetAdapterFor(c.GetType());
                var view = adapter.GetElements(c).FirstOrDefault(e => e.DataContext.GetType() == viewModel);
                if (view != null)
                {
                    adapter.Remove(c, view);
                }
            });

            return;
        }

        var adapter = _viewAdapters.GetAdapterFor(container.GetType());
        var view = adapter.GetElements(container).FirstOrDefault(e => e.DataContext.Equals(viewModel));
        if (view != null)
        {
            adapter.Remove(container, view);
        }
    }
}
