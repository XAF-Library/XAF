using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPlugin.ViewModels;
using XAF.UI.Abstraction.Commands;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;
using XAF.UI.Commands;
using XAF.UI.ReactiveCommands;

namespace WpfPluginApp.ViewModels;
public class ControlViewModel : XafViewModel
{
    public RxCommand NavigateToViewACommand { get; }

    public RxCommand NavigateForwardCommand { get; }

    public RxCommand NavigateBackCommand { get; }

    public IXafCommand OpenDialogCommand { get; }

    public ControlViewModel(INavigationService navigationService, IWindowService windowService)
    {
        var viewAVm = new ViewAViewModel(navigationService);
        // Create a command that executes a Navigation.
        NavigateToViewACommand = RxCommand.CreateFromTask(() => navigationService.NavigateAsync(viewAVm, "PageViews"));

        OpenDialogCommand = XafCommand.Create(() => windowService.ShowDialogAsync<DialogViewModel, string>("Navigated"));
        NavigateBackCommand = RxCommand.CreateFromTask(() => navigationService.NavigateBack("PageViews"), navigationService.CanNavigateBack("PageViews"));
        NavigateForwardCommand = RxCommand.CreateFromTask(() => navigationService.NavigateForward("PageViews"), navigationService.CanNavigateForward("PageViews"));
    }
}
