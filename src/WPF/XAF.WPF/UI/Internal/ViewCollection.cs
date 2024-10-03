using DynamicData;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using XAF.Core.MVVM;

namespace XAF.WPF.UI.Internal;
internal class ViewCollection : IViewCollection
{
    private readonly Dictionary<IXafViewModel, FrameworkElement> _views;
    private readonly BehaviorSubject<IComparer<IXafViewModel>> _comparerSubject;
    private readonly BehaviorSubject<Func<IXafViewModel, bool>> _filterSubject;
    private readonly SourceList<(IXafViewModel, FrameworkElement)> _sourceList;

    public ViewCollection()
    {
        _views = new();
        _comparerSubject = new(Comparer<IXafViewModel>.Default);
        _sourceList = new();
    }

    public FrameworkElement this[IXafViewModel key]
    {
        get => _views[key];
        set => Add(key, value);
    }

    public IComparer<IXafViewModel> Sort
    {
        get => _comparerSubject.Value;
        set => _comparerSubject.OnNext(value);
    }
    public Func<IXafViewModel, bool> Filter { get; set; }

    public ICollection<IXafViewModel> Keys => _views.Keys;

    public ICollection<FrameworkElement> Values => _views.Values;
    public int Count => _views.Count;
    public bool IsReadOnly => false;

    public void Add(IXafViewModel key, FrameworkElement value)
    {
        _views.Add(key, value);
        _sourceList.Add((key, value));
    }

    public void Add(KeyValuePair<IXafViewModel, FrameworkElement> item)
    {
        _views.Add(item.Key, item.Value);
        _sourceList.Add((item.Key, item.Value));
    }

    public void Clear()
    {
        _views.Clear();
        _sourceList.Clear();
    }

    public bool Contains(KeyValuePair<IXafViewModel, FrameworkElement> item)
    {
        return _views.Contains(item);
    }

    public bool ContainsKey(IXafViewModel key)
    {
        return _views.ContainsKey(key);
    }

    public void CopyTo(KeyValuePair<IXafViewModel, FrameworkElement>[] array, int arrayIndex)
    {
        ((IDictionary<IXafViewModel, FrameworkElement>)_views).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<IXafViewModel, FrameworkElement>> GetEnumerator()
    {
        return _views.GetEnumerator();
    }

    public bool Remove(IXafViewModel key)
    {
        if (!_views.Remove(key, out var value))
        {
            return false;
        }

        _sourceList.Remove((key, value));
        return true;
    }

    public bool Remove(KeyValuePair<IXafViewModel, FrameworkElement> item)
    {
        if (!_views.Remove(item.Key))
        {
            return false;
        }

        _sourceList.Remove((item.Key, item.Value));
        return true;
    }

    public IDisposable Subscribe(IObserver<IChangeSet<(IXafViewModel viewModel, FrameworkElement view)>> observer)
    {
        Func<IXafViewModel, bool> f;

        return _sourceList
            .Connect()
            .Filter(_filterSubject.Select(ExtendFilter))
            .Sort(_comparerSubject.Select(c => new XafViewComparer(c)))
            .Subscribe(observer);
    }

    public bool TryGetValue(IXafViewModel key, [MaybeNullWhen(false)] out FrameworkElement value)
    {
        return _views.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _views.GetEnumerator();
    }

    private static Func<(IXafViewModel, FrameworkElement), bool> ExtendFilter(Func<IXafViewModel, bool> filter)
        => (t1) => filter(t1.Item1);

    private class XafViewComparer : Comparer<(IXafViewModel, FrameworkElement)>
    {
        private readonly IComparer<IXafViewModel> _coreComparer;

        public XafViewComparer(IComparer<IXafViewModel> coreComparer)
        {
            _coreComparer = coreComparer;
        }

        public override int Compare((IXafViewModel, FrameworkElement) x, (IXafViewModel, FrameworkElement) y)
        {
            return _coreComparer.Compare(x.Item1, y.Item1);
        }
    }
}
