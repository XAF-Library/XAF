using DynamicData;
using System.Reactive.Disposables;
using System.Windows.Controls;
using XAF.Core.ExtensionMethods;

namespace XAF.WPF.UI.ViewAdapters;
internal class ListBoxAdapter : ViewAdapter<ListBox>
{
    public override IDisposable Attach(ListBox container, IViewModelPresenter presenter)
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
            .OnItemAdded(i => container.SelectedItems.Add(i.view))
            .OnItemRemoved(i => container.SelectedItems.Remove(i.view))
            .Subscribe()
            .DisposeWith(disposables);

        return disposables;
    }
}
