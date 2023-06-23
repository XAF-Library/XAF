using System.Windows;

namespace XAF.UI.Abstraction;
public interface ISplashWindowViewModel : IViewModel
{
    Type WindowType { get; }
    Task OnAppStartAsync();
    Task AfterModuleInitializationAsync();
}
