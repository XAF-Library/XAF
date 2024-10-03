using System.Windows;

namespace XAF.WPF.UI.ViewAdapters;

public interface IViewAdapter
{
    Type ContainerType { get; }

    IDisposable Attach(FrameworkElement container, IViewModelPresenter presenter);
}