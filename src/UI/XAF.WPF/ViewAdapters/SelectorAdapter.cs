using DynamicData;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ExtensionMethods;
using XAF.UI.ViewComposition;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.ViewAdapters;
public class SelectorAdapter : SingleActiveViewPresenter<Selector>
{
    public override void Connect(Selector view)
    {
        Views.Connect()
            .PrepareForViewChange(this)
            .Bind(out var views)
            .Subscribe()
            .DisposeWith(Disposables);
        view.ItemsSource = views;

        ActiveViews.Connect()
            .PrepareForViewChange(this)
            .QueryWhenChanged(c => view.SelectedItem = c.FirstOrDefault()?.View)
            .Subscribe();
    }
}
