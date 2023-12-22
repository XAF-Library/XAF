using System.Windows;
using WpfPluginApp.ViewModels;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.Attributes;

namespace WpfPluginApp.Views;
/// <summary>
/// Interaktionslogik für Shell.xaml
/// </summary>
[Shell]
[ViewFor<ShellViewModel>]
public partial class Shell : Window
{
    public Shell(IViewService compositionService)
    {
        InitializeComponent();
        compositionService.AddViewAsync<ControlViewModel>("ControlView").Wait();
    }
}
