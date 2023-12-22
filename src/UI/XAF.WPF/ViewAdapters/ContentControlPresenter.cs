using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Controls;
using XAF.UI.Abstraction.ExtensionMethods;
using XAF.UI.ViewComposition;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.ViewAdapters;
public sealed class ContentControlAdapter : ViewAdapter<ContentControl, SingleActiveViewPresenter>
{
    public override void Adapt(ContentControl view, SingleActiveViewPresenter viewPresenter, CompositeDisposable disposables)
    {
        viewPresenter.ActiveViews
            .Connect()
            .PrepareForViewChange(viewPresenter)
            .QueryWhenChanged(c => view.Content = c.FirstOrDefault()?.View)
            .Subscribe()
            .DisposeWith(disposables);
    }
}
