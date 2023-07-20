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

    public virtual void OnNavigatedTo(object? parameter)
    {
    }
}

public abstract class NavigationTarget<T> : NavigationTarget, INavigationTarget<T>
{
    public override void OnNavigatedTo(object? parameter)
    {
        if (parameter is T tParameter)
        {
            OnNavigatedTo(tParameter);
            return;
        }
    }
    public abstract void OnNavigatedTo(T parameter);
}
