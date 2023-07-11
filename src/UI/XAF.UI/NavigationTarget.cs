using XAF.UI.Abstraction;

namespace XAF.UI;
public abstract class NavigationTarget : ViewModelBase, INavigationTarget
{
    public virtual void OnNavigatedFrom()
    {

    }

    public virtual void OnNavigatedTo()
    {

    }
}

public abstract class NavigationTarget<T> : NavigationTarget, INavigationTarget<T>
{
    public virtual void OnNavigatedTo(T parameter)
    {

    }
}
