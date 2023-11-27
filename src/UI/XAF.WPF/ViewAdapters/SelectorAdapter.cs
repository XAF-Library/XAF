using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls.Primitives;
using XAF.UI.Abstraction.ExtensionMethods;
using XAF.UI.ViewComposition;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.ViewAdapters;
public class SelectorAdapter : ViewAdapter<Selector, SingleActiveViewPresenter>
{
    public override void Adapt(Selector view, SingleActiveViewPresenter viewPresenter, CompositeDisposable disposables)
    {
        viewPresenter.Views.Connect()
            .PrepareForViewChange(viewPresenter)
            .Bind(out var views)
            .Subscribe()
            .DisposeWith(disposables);
        view.ItemsSource = views;

        viewPresenter.ActiveViews.Connect()
            .PrepareForViewChange(viewPresenter)
            .QueryWhenChanged(c => view.SelectedItem = c.FirstOrDefault()?.View)
            .Subscribe()
            .DisposeWith(disposables);
    }
}
