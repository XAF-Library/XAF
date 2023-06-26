using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPlugin.ViewModels;
using XAF.UI;
using XAF.UI.Abstraction;
using XAF.UI.Reactive.Commands;

namespace WpfPluginApp.ViewModels;
public class ControlViewModel : ViewModelBase
{
    public RxCommand NavigateToViewACommand { get; }

    public ControlViewModel(INavigationService navigationService)
    {
        var viewAVm = new ViewAViewModel(navigationService);
        // Create a command that executes a Navigation.
        NavigateToViewACommand = RxCommand.Create(() => navigationService.NavigateTo("PageViews", viewAVm));
    }
}
