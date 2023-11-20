using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public abstract class ViewPresenter<TView> : IViewPresenter<TView> where TView : class
{
    private readonly SourceList<IXafViewBundle> _views;
    private readonly SourceList<IXafViewBundle> _activeViews;
    private bool _disposedValue;

    public CompositeDisposable Disposables { get; }
    public ISubject<IComparer<IXafViewBundle>> Comparer { get; }
    public IObservableList<IXafViewBundle> Views => _views;
    public IObservableList<IXafViewBundle> ActiveViews => _activeViews;

    protected ViewPresenter()
    {
        Comparer = new BehaviorSubject<IComparer<IXafViewBundle>>(Comparer<IXafViewBundle>.Default);
        Disposables = new CompositeDisposable();
        _views = new SourceList<IXafViewBundle>();
        _activeViews = new SourceList<IXafViewBundle>();
    }

    public abstract void Connect(TView view);

    public virtual void Add(IXafViewBundle view)
    {
        if (!_views.Items.Contains(view))
        {
            _views.Add(view);
        }
    }

    public virtual void Remove(IXafViewBundle view)
    {
        _activeViews.Remove(view);
        _views.Remove(view);
    }

    public virtual void Activate(IXafViewBundle view)
    {
        if (!_views.Items.Contains(view))
        {
            _views.Add(view);
        }

        _activeViews.Add(view);
    }

    public virtual void Deactivate(IXafViewBundle view)
    {
        _activeViews.Remove(view);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                Disposables.Dispose();
            }

            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
