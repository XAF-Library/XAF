using System.Windows;
using XAF.Utilities;

namespace XAF.UI.WPF.Internal;
public class NavigationBackStack : NotifyPropertyChanged
{

    private NavigationEntry? _current;
    private NavigationEntry? _root;
    private NavigationEntry? _tail;

    private uint _count;

    public uint Capacity { get; set; }

    private bool _canNavigateBack;

    public bool CanNavigateBack
    {
        get { return _canNavigateBack; }
        set => Set(ref _canNavigateBack, value);
    }

    private bool _canNavigateForward;

    public bool CanNavigateForward
    {
        get { return _canNavigateForward; }
        set => Set(ref _canNavigateForward, value);
    }


    public void Insert(FrameworkElement element)
    {
        if (Capacity <= 1)
        {
            return;
        }

        var entry = new NavigationEntry(element);
        _current = entry;

        switch (_count)
        {
            case 0:
                _root = entry;
                _tail = entry;
                break;
            case 1:
                _tail = entry;
                _root.After = _tail;
                _tail.Before = _root;
                break;
            default:
                _tail.After = entry;
                entry.Before = _tail;
                _tail = entry;
                break;
        }

        _count++;

        if (_count > Capacity)
        {

            _root = _root.After;
            _root.Before = null;
            _count--;
        }

        CanNavigateBack = _current.Before != null;
    }

    public NavigationBackStack(uint capacity = 10)
    {
        Capacity = capacity;
    }

    public NavigationEntry NavigateBack()
    {
        if (!CanNavigateBack)
        {
            throw new InvalidOperationException("cant navigate back");
        }

        _current = _current.Before;

        CanNavigateBack = _current.Before != null;
        CanNavigateForward = _current.After != null;

        return _current;
    }

    public NavigationEntry NavigateForward()
    {
        if (!CanNavigateForward)
        {
            throw new InvalidOperationException("cant navigate back");
        }

        _current = _current.After;

        CanNavigateForward = _current.After != null;
        CanNavigateBack = _current.Before != null;

        return _current;
    }
}
