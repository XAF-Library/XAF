using System.Threading.Tasks;
using WpfPlugin.ViewModels;
using XAF.UI;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace WpfPluginApp.ViewModels;
public class ShellViewModel : XafViewModel
{
    private readonly IViewService _viewService;

    public ShellViewModel(IViewService viewService)
    {
        _viewService = viewService;
    }

    public override async Task LoadAsync()
    {
        await Task.Delay(2000);
        await _viewService.AddViewAsync<StackMessageViewModel>("StackPanel");
        await _viewService.AddViewAsync<StackMessageViewModel>("StackPanel");
    }
}
