using System.Windows;
using WpfPluginApp.ViewModels;
using XAF.UI.Abstraction;
using XAF.UI.WPF.Attributes;

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
