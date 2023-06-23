using WpfPlugin.ViewModels;
using XAF.WPF.Attributes;

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
