using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public class ViewPresenter : IViewPresenter
{
    private readonly SourceList<IXafBundle> _views;
    private readonly SourceList<IXafBundle> _activeViews;
    private readonly IViewAdapterCollection _viewAdapters;
    private bool _disposedValue;
    private readonly Dictionary<object, CompositeDisposable> _viewDisposables;

    public ISubject<IComparer<IXafBundle>> Comparer { get; }
    public IObservableList<IXafBundle> Views => _views;
    public IObservableList<IXafBundle> ActiveViews => _activeViews;

    public IBundleProvider BundleProvider { get; }

    public ViewPresenter(IViewAdapterCollection viewAdapters, IBundleProvider bundleProvider)
    {
        Comparer = new BehaviorSubject<IComparer<IXafBundle>>(Comparer<IXafBundle>.Default);

        _viewDisposables = new();
        _views = new SourceList<IXafBundle>();
        _activeViews = new SourceList<IXafBundle>();
        _viewAdapters = viewAdapters;
        BundleProvider = bundleProvider;
    }

    public virtual void Add(IXafBundle view)
    {
        if (!_views.Items.Contains(view))
        {
            _views.Add(view);
        }
    }

    public virtual void Remove(IXafBundle view)
    {
        _activeViews.Remove(view);
        _views.Remove(view);
    }

    public virtual void Activate(IXafBundle view)
    {
        if (!_views.Items.Contains(view))
        {
            _views.Add(view);
        }

        _activeViews.Add(view);
    }

    public virtual void Deactivate(IXafBundle view)
    {
        _activeViews.Remove(view);
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
        if(_viewDisposables.TryGetValue(view, out var disposables))
        {
            disposables.Dispose();
            _viewDisposables.Remove(view);
        }
    }
}
