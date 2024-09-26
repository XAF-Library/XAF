using System.Reactive.Linq;
using System.Windows.Controls;
using XAF.WPF.ExtenionMethods;

namespace XAF.WPF.UI.Internal;
public class ViewAdapter
{
    public IDisposable Attach(ContentControl container, IViewModelPresenter presenter)
    {
        return presenter.SelectedViews.First()
            .Subscribe(view => container.Content = view);
    }
}
