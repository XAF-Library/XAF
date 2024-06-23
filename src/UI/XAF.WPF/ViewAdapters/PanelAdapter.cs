using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XAF.UI.Abstraction.ExtensionMethods;
using XAF.UI.ViewComposition;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.ViewAdapters;
public class PanelAdapter : ViewAdapter<Panel, AllActiveViewPresenter>
{
    public override void Adapt(Panel view, AllActiveViewPresenter viewPresenter, CompositeDisposable disposables)
    {
        viewPresenter.ActiveViews
            .Connect()
            .PrepareForViewChange(viewPresenter)
            .QueryWhenChanged()
            .Subscribe(c =>
            {
                view.Children.Clear();
                foreach (var e in c.Select(b => b.View).OfType<UIElement>()) 
                {
                    view.Children.Add(e);
                }
            })
            .DisposeWith(disposables);
    }
}
