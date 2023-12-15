using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Automation.Peers;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ViewComposition.Internal;

namespace XAF.UI.WPF.Behaviors;
public class ViewContainer : Behavior<FrameworkElement>
{
    public static readonly DependencyProperty KeyProperty =
        DependencyProperty.Register("Key", typeof(object), typeof(ViewContainer), new PropertyMetadata(null));

    public object Key
    {
        get { return GetValue(KeyProperty); }
        set { SetValue(KeyProperty, value); }
    }

    protected override void OnAttached()
    {
        base.OnAttached();

        if (Key is null)
        {
            throw new NotSupportedException("the key was not set");
        }

        var viewService = InternalServiceProvider.Current.GetRequiredService<IViewService>();
        var viewAdapterCollection = InternalServiceProvider.Current.GetRequiredService<IViewAdapterCollection>();
        var adapter = viewAdapterCollection.GetAdapterFor(AssociatedObject.GetType());
        var presenter = (IViewPresenter)ActivatorUtilities.CreateInstance(InternalServiceProvider.Current, adapter.PresenterType);
        viewService.AddPresenter(presenter, Key);

        presenter.Connect(AssociatedObject);
    }
}
