using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Windows;
using XAF.Core.MVVM;

namespace XAF.WPF.UI.Internal;
internal class ViewModelPresenter : IViewModelPresenter
{
    private List<FrameworkElement> _containers;
    private ConcurrentDictionary<FrameworkElement, IDisposable> _disposablesForAdapters;

    private readonly IViewAdapterProvider _viewAdapterProvider;
    private readonly IViewProvider _viewProvider;
    private readonly ILogger<ViewModelPresenter> _logger;

    public object Key { get; }
    public IViewCollection SelectedViews { get; }
    public IViewCollection Views { get; }

    public ViewModelPresenter(object key, IViewAdapterProvider viewAdapterProvider, IViewProvider viewProvider, ILogger<ViewModelPresenter> logger)
    {
        Key = key;
        _viewAdapterProvider = viewAdapterProvider;
        _viewProvider = viewProvider;
        _logger = logger;

        _containers = [];
        _disposablesForAdapters = [];
    }

    public void Add<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public void Remove<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public void Select<TViewModel>(TViewModel vm) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public void AttachTo(FrameworkElement container)
    {
        if (!_viewAdapterProvider.TryGetAdapterFor(container, out var adapter))
        {
            _logger.LogError($"No ViewAdapter for {container} found");
            return;
        }

        var disposable = adapter.Attach(container, this);

        if (!_disposablesForAdapters.TryAdd(container, disposable))
        {
            _logger.LogError($"adapter disposable for {container} could not be added");
            disposable.Dispose();
            return;
        }

        _containers.Add(container);
    }

    public void DetachFrom(FrameworkElement container)
    {
        if (!_containers.Remove(container))
        {
            return;
        }

        if (_disposablesForAdapters.TryRemove(container, out var disposable))
        {
            disposable.Dispose();
        }
    }
}
