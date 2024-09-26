using System.Windows;

namespace XAF.WPF.UI;

public interface IViewAdapter
{
    Type ContainerType { get; }

    IDisposable Attach(FrameworkElement container, IViewModelPresenter presenter);
}