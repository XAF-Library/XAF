using System.Reactive.Subjects;
using System.Reflection;
using System.Reflection.Metadata;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.ViewComposition.Internal;

internal class NavigationStack
{
    private readonly Stack<NavigationEntry> _backStack = new();
    private readonly Stack<NavigationEntry> _forwardStack = new();
    private readonly Subject<bool> _canNavigateBack = new();
    private readonly Subject<bool> _canNavigateForward = new();
    public NavigationEntry? CurrentEntry { get; private set; }

    public IObservable<bool> CanNavigateBack()
        => _canNavigateBack;

    public IObservable<bool> CanNavigateForward()
        => _canNavigateForward;

    public NavigationEntry NavigateBack()
    {
        var entry = _backStack.Pop();
        
        if(CurrentEntry != null)
        {
            _forwardStack.Push(CurrentEntry);
            _canNavigateForward.OnNext(true);
        }

        _canNavigateBack.OnNext(_backStack.Count != 0);
        CurrentEntry = entry;
        return entry;
    }

    public NavigationEntry NavigateForward()
    {
        var entry = _forwardStack.Pop();
        _canNavigateForward.OnNext(_forwardStack.Count != 0);

        if(CurrentEntry != null)
        {
            _backStack.Push(CurrentEntry);
            _canNavigateBack.OnNext(true);
        }

        CurrentEntry = entry;
        return entry;
    }

    public void RecordNavigation(IXafBundle bundle, object? parameter)
    {
        var entry = new NavigationEntry(bundle, parameter);
        if (CurrentEntry != null)
        {
            _backStack.Push(CurrentEntry);
            _canNavigateBack.OnNext(true);
        }

        _forwardStack.Clear();
        _canNavigateForward.OnNext(false);
        CurrentEntry = entry;
    }
}