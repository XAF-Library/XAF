using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfPluginApp.ViewModels;
using XAF.Modularity;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.WPF.ViewComposition;

namespace WpfPluginApp.Views;
/// <summary>
/// Interaktionslogik für SplashWindow.xaml
/// </summary>
[StartupWindow<SplashWindowViewModel>]
public partial class SplashScreen : Window
{
    public SplashScreen()
    {
        InitializeComponent();
    }
}
