using System.Windows;
using WpfPluginApp.ViewModels;
using XAF;
using XAF.WPF.Attributes;

namespace WpfPluginApp.Views;
/// <summary>
/// Interaktionslogik für Shell.xaml
/// </summary>
[Shell]
[ViewFor<ShellViewModel>]
[ContainsViewContainer("PageViews")]
[ContainsViewContainer("ControlView")]
public partial class Shell : Window
{
    public Shell(IViewCompositionService compositionService)
    {
        InitializeComponent();
        compositionService.InsertView<ControlViewModel>("ControlView");
    }
}
