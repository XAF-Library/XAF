using DynamicData;
using System.Reactive.Linq;
using System.Windows.Controls;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ExtensionMethods;
using XAF.UI.ViewComposition;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.ViewAdapters;
public sealed class ContentControlPresenter : SingleActiveViewPresenter<ContentControl>
{
    public override void Connect(ContentControl view)
    {
        ActiveViews
            .Connect()
            .PrepareForViewChange(this)
            .QueryWhenChanged(c => view.Content = c.FirstOrDefault()?.View)
            .Subscribe()
            .DisposeWith(Disposables);
    }
}
