using System.Windows;
using XAF.Core.MVVM;

namespace XAF.WPF.UI;
public interface IViewModelPresenter
{
    object Key { get; }

    IViewCollection SelectedViews { get; }

    IViewCollection Views { get; }

    void AttachTo(FrameworkElement container);

    void DetachFrom(FrameworkElement container);

    void Add<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    void Select<TViewModel>(TViewModel vm)
        where TViewModel : IXafViewModel;

    void Remove<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;
}
