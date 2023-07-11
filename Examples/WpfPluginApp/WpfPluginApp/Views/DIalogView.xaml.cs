using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfPluginApp.ViewModels;
using XAF.UI.WPF.Attributes;

namespace WpfPluginApp.Views;
/// <summary>
/// Interaktionslogik für DIalogView.xaml
/// </summary>
[ViewFor<TestDialogViewModel>]
public partial class DIalogView : UserControl
{
    public DIalogView()
    {
        InitializeComponent();
    }
}
