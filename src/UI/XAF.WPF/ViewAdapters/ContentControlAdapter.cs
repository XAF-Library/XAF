using DynamicData;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Resources.Extensions;
using System.Windows;
using System.Windows.Controls;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.ViewComposition;
using XAF.UI.WPF.ExtensionMethodes;
using XAF.UI.WPF.ExtensionMethods;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.ViewAdapters;
public sealed class ContentControlAdapter : ViewAdapterBase<ContentControl>
{
    public override void Adapt(IViewPresenter presenter, ContentControl target, CompositeDisposable disposables)
    {
        if(target.Content != null || target.HasBinding(ContentControl.ContentProperty))
        {
            throw new InvalidOperationException("the ContentControl's Content property is not empty");
        }
        
        presenter.ActiveViews
            .Connect()
            .Sort(presenter.Comparer)
            .QueryWhenChanged()
            .Subscribe(c => target.Content = c.FirstOrDefault())
            .DiposeWith(disposables);
    }
}
