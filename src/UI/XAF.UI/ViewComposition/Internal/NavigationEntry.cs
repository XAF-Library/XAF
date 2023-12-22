using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.ViewComposition.Internal;

internal class NavigationEntry
{
    public IXafBundle Bundle { get; }
    public object? Parameter { get; }

    public NavigationEntry(IXafBundle bundle, object? parameter)
    {
        Bundle = bundle;
        Parameter = parameter;
    }
}