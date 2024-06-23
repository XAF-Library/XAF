using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public class ViewPresenter : IViewPresenter
{
    private readonly IViewAdapterCollection _viewAdapters;
    private bool _disposedValue;
    private readonly Dictionary<object, CompositeDisposable> _viewDisposables;

    protected SourceList<IXafBundle> ViewsSource { get; }
    protected SourceList<IXafBundle> ActiveViewsSource { get; }

    public ISubject<IComparer<IXafBundle>> Comparer { get; }
    public IObservableList<IXafBundle> Views => ViewsSource;
    public IObservableList<IXafBundle> ActiveViews => ActiveViewsSource;

    public IBundleProvider BundleProvider { get; }

    public ViewPresenter(IViewAdapterCollection viewAdapters, IBundleProvider bundleProvider)
    {
        var comparer = Comparer<IXafBundle>.Default;
        Comparer = new BehaviorSubject<IComparer<IXafBundle>>(comparer);

        _viewDisposables = new();
        ViewsSource = new SourceList<IXafBundle>();
        ActiveViewsSource = new SourceList<IXafBundle>();
        _viewAdapters = viewAdapters;
        BundleProvider = bundleProvider;
    }

    public virtual Task AddAsync(IXafBundle view)
    {
        if (!ViewsSource.Items.Contains(view))
        {
            ViewsSource.Add(view);
            BundleProvider.AddBundle(view);
        }

        return Task.CompletedTask;
    }

    public virtual async Task<bool> RemoveAsync(IXafBundle view)
    {
        var result = ViewsSource.Remove(view);
        await DeactivateAsync(view);
        return result;
    }

    public virtual async Task ActivateAsync(IXafBundle view)
    {
        if (!ViewsSource.Items.Contains(view))
        {
            ViewsSource.Add(view);
        }

        if (!ActiveViews.Items.Contains(view))
        {
            ActiveViewsSource.Add(view);
            await view.ViewModel.LoadAsync().ConfigureAwait(false);
        }
    }

    public virtual async Task<bool> DeactivateAsync(IXafBundle view)
    {
        var result = ActiveViewsSource.Remove(view);

        if (result)
        {
            await view.ViewModel.UnloadAsync().ConfigureAwait(false);
        }

        return result;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                foreach (var disposables in _viewDisposables.Values)
                {
                    disposables.Dispose();
                }
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Connect(object view)
    {
        var disposables = new CompositeDisposable();
        _viewDisposables.Add(view, disposables);
        var adapter = _viewAdapters.GetAdapterFor(view.GetType());
        adapter.Adapt(view, this, disposables);
    }

    public void Disconnect(object view)
    {
        if (_viewDisposables.TryGetValue(view, out var disposables))
        {
            disposables.Dispose();
            _viewDisposables.Remove(view);
        }
    }
}
