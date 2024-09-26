using System.Collections.ObjectModel;
using System.Windows;
using XAF.Core.MVVM;

namespace XAF.WPF.UI;
public interface IViewCollection
{
    IComparer<IXafViewModel> Sort { get; set; }
    Func<IXafViewModel, bool> Filter { get; set; }

    IObservable<IReadOnlyList<FrameworkElement>> WhenChanged();

    IDisposable Bind(out ReadOnlyObservableCollection<FrameworkElement> collection);
}
