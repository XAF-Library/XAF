using DynamicData;
using System.Windows;
using XAF.Core.MVVM;

namespace XAF.WPF.UI;
public interface IViewCollection : IDictionary<IXafViewModel, FrameworkElement>, IObservable<IChangeSet<(IXafViewModel viewModel, FrameworkElement view)>>
{
    IComparer<IXafViewModel> Sort { get; set; }
    Func<IXafViewModel, bool> Filter { get; set; }
}
