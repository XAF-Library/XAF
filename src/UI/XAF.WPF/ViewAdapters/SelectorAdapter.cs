using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
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
            .Transform(b => b.View)
            .Bind(out var views)
            .Subscribe()
            .DisposeWith(disposables);
        view.ItemsSource = views;

        viewPresenter.ActiveViews.Connect()
            .PrepareForViewChange(viewPresenter)
            .QueryWhenChanged()
            .Subscribe(c => view.SelectedItem = c.FirstOrDefault()?.View)
            .DisposeWith(disposables);
    }
}
