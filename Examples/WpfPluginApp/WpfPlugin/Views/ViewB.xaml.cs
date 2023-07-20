using WpfPlugin.ViewModels;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.WPF.Attributes;

namespace WpfPlugin.Views;
/// <summary>
/// Interaktionslogik für ViewB.xaml
/// </summary>
[ViewFor<ViewBViewModel>]
public partial class ViewB
{
    public ViewB()
    {
        InitializeComponent();
    }
}
