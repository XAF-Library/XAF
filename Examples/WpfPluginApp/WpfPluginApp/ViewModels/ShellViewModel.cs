using System.Threading.Tasks;
using XAF.UI;
using XAF.UI.ViewModels;

namespace WpfPluginApp.ViewModels;
public class ShellViewModel : XafViewModel
{
    public override async Task LoadAsync()
    {
        await Task.Delay(200);
    }
}
