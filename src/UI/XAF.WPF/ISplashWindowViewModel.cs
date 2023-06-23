using System.Windows;
using XAF;

namespace XAF.WPF;
public interface ISplashWindowViewModel : IViewModel
{
    Type WindowType { get; }
    Window? SplashWindow { get; set; }
    Task OnAppStartAsync();
    Task AfterModuleInitalizationAsync();
}
