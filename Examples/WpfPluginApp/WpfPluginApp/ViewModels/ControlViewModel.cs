using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPlugin.ViewModels;
using XAF.UI;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Commands;
using XAF.UI.Abstraction.Dialog;
using XAF.UI.Commands;
using XAF.UI.Reactive.Commands;

namespace WpfPluginApp.ViewModels;
public class ControlViewModel : ViewModelBase
{
    public RxCommand NavigateToViewACommand { get; }

    public IXafCommand OpenDialogCommand { get; }

    public ControlViewModel(INavigationService navigationService, IDialogService dialogService)
    {
        var viewAVm = new ViewAViewModel(navigationService);
        // Create a command that executes a Navigation.
        NavigateToViewACommand = RxCommand.Create(() => navigationService.NavigateTo("PageViews", viewAVm));

        OpenDialogCommand = XafCommand.CreateCommand(() => dialogService.ShowInputDialog<TestDialogViewModel, string, string>("Test"));
    }
}
