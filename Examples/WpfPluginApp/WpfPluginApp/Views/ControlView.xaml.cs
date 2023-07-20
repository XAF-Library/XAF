using System.Windows.Controls;
using WpfPluginApp.ViewModels;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.WPF.Attributes;

namespace WpfPluginApp.Views;
/// <summary>
/// Interaktionslogik für ControlView.xaml
/// </summary>
[ViewFor<ControlViewModel>]
public partial class ControlView : UserControl
{
    public ControlView()
    {
        InitializeComponent();
    }
}
