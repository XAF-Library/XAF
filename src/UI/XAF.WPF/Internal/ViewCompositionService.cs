using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.WPF.Behaviors;
using XAF.UI.WPF.ExtensionMethodes;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Internal;
internal class ViewCompositionService : IViewCompositionService
{
    private readonly IViewDescriptorProvider _viewDescriptorProvider;
    private readonly IViewAdapterCollection _viewAdapters;
    private readonly IViewProvider _viewProvider;
    private readonly HashSet<object> _aviableNavKey = new();

    public ViewCompositionService(IViewAdapterCollection viewAdapters, IViewProvider viewProvider)
    {
        _viewDescriptorProvider = viewProvider.ViewDescriptorProvider;
        _viewAdapters = viewAdapters;
        _viewProvider = viewProvider;

        var navKeys = _viewDescriptorProvider.GetDescriptorsByDecorator<ContainsViewContainerAttribute>()
          .SelectMany(d => d.GetNavigationKeys());

        foreach (var navKey in navKeys)
        {
            _aviableNavKey.Add(navKey);
        }
    }

    public void ClearViews(object containerKey)
    {
        if (!_aviableNavKey.Contains(containerKey))
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
        if (!_aviableNavKey.Contains(containerKey))
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
        if (!_aviableNavKey.Contains(containerKey))
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
        if (!_aviableNavKey.Contains(containerKey))
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
        if (!_aviableNavKey.Contains(containerKey))
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
