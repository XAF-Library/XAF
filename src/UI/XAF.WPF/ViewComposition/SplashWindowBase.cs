using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XAF.UI.WPF.ViewComposition;
public class SplashWindow : Window
{
    public virtual Task OnAppLoadAsync()
    {
        return Task.CompletedTask;
    }
}
