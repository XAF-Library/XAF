using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public abstract class ViewPresenter<TView> : IViewPresenter where TView : class
{
    private readonly SourceList<IXafView> _views;
    private readonly SourceList<IXafView> _activeViews;
    private bool _disposedValue;

    public object Key { get; }
    public CompositeDisposable Disposables { get; }
    public ISubject<IComparer<IXafView>> Comparer { get; }
    public IObservableList<IXafView> Views => _views;
    public IObservableList<IXafView> ActiveViews => _activeViews;
    public TView Target { get; }

    object IViewPresenter.Target => Target;

    protected ViewPresenter(object key, TView targetView)
    {
        Key = key;
        Target = targetView;

        Comparer = new BehaviorSubject<IComparer<IXafView>>(Comparer<IXafView>.Default);
        Disposables = new CompositeDisposable();
        _views = new SourceList<IXafView>();
        _activeViews = new SourceList<IXafView>();
    }

    public virtual void Connect(object targetView)
    {
        Connect(Target, 
            Views.Connect().Sort(Comparer),
            ActiveViews.Connect().Sort(Comparer));
    }

    public abstract void Connect(
        TView view,
        IObservable<IChangeSet<IXafView>> views,
        IObservable<IChangeSet<IXafView>> activeViews);

    public virtual void Add(IXafView view)
    {
        if (!_views.Items.Contains(view))
        {
            _views.Add(view);
        }
    }

    public virtual void Remove(IXafView view)
    {
        _activeViews.Remove(view);
        _views.Remove(view);
    }

    public virtual void Activate(IXafView view)
    {
        if (!_views.Items.Contains(view))
        {
            _views.Add(view);
        }

        _activeViews.Add(view);
    }

    public virtual void Deactivate(IXafView view)
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
