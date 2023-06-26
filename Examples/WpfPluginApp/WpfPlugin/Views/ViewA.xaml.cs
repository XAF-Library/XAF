using WpfPlugin.ViewModels;
using XAF.UI.WPF.Attributes;

namespace WpfPlugin.Views;
/// <summary>
/// Interaktionslogik für ViewA.xaml
/// </summary>
[ViewFor<ViewAViewModel>]
public partial class ViewA
{
    public ViewA()
    {
        InitializeComponent();
    }
}
