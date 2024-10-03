using DynamicData;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls.Primitives;
using XAF.Core.ExtensionMethods;
using XAF.Core.MVVM;

namespace XAF.WPF.UI.ViewAdapters;
internal class SelectorAdapter : ViewAdapter<Selector>
{
    public override IDisposable Attach(Selector container, IViewModelPresenter presenter)
    {
        var disposables = new CompositeDisposable();

        presenter.Views
            .Transform(i => i.view)
            .Bind(out var views)
            .Subscribe()
            .DisposeWith(disposables);

        container.ItemsSource = views;

        Disposable.Create(() => container.ItemsSource = null)
            .DisposeWith(disposables);

        presenter.SelectedViews
            .OnItemAdded(i => ItemSelected(presenter, i.viewModel, i.view, container))
            .Subscribe()
            .DisposeWith(disposables);

        return disposables;
    }

    private void ItemSelected(IViewModelPresenter presenter, IXafViewModel viewModel, FrameworkElement view, Selector container)
    {
        presenter.Remove(viewModel, CancellationToken.None);
        container.SelectedItem = view;
    }
}
