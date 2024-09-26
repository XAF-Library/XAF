using System.Windows;

namespace XAF.WPF.UI;
public abstract class ViewAdapter<T> : IViewAdapter
    where T : FrameworkElement
{
    public Type ContainerType { get; } = typeof(T);

    public IDisposable Attach(FrameworkElement container, IViewModelPresenter presenter)
    {
        if (!container.GetType().IsAssignableTo(typeof(T)))
        {
            throw new InvalidOperationException($"Adapter is not compatible with container of type {container.GetType()}");
        }

        return Attach((T)container, presenter);
    }

    public abstract IDisposable Attach(T container, IViewModelPresenter presenter);
}
