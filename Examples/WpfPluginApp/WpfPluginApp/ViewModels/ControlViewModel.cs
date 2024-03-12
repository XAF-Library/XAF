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
using XAF.UI.Commands.Internal;
using XAF.UI.ReactiveCommands;

namespace WpfPluginApp.ViewModels;
public class ControlViewModel : XafViewModel
{
    private readonly IWindowService _windowService;

    public RxCommand NavigateToViewACommand { get; }
    
    public RxCommand NavigateToViewBCommand { get; }

    public RxCommand NavigateForwardCommand { get; }

    public RxCommand NavigateBackCommand { get; }

    public IXafCommand OpenDialogCommand { get; }

    public IXafCommand DeleteCommand { get; }

    public ControlViewModel(INavigationService navigationService, IWindowService windowService, IViewService viewService)
    {
        var viewAVm = new ViewAViewModel(navigationService);
        // Create a command that executes a Navigation.
        NavigateToViewACommand = RxCommand.CreateFromTask(() => navigationService.NavigateAsync(viewAVm, "PageViews"));
        NavigateToViewBCommand = RxCommand.CreateFromTask(() => navigationService.NavigateAsync(typeof(ViewBViewModel), "Test", "PageViews"));
        OpenDialogCommand = XafCommand.Create(OpenDialog, new AsyncCommandOptions());
        NavigateBackCommand = RxCommand.CreateFromTask(() => navigationService.NavigateBack("PageViews"), navigationService.CanNavigateBack("PageViews"));
        NavigateForwardCommand = RxCommand.CreateFromTask(() => navigationService.NavigateForward("PageViews"), navigationService.CanNavigateForward("PageViews"));
        DeleteCommand = XafCommand.Create(() => viewService.RemoveAllAsync("StackPanel"));
        _windowService = windowService;
    }

    private async Task OpenDialog()
    {
        var vm = await _windowService.ShowDialogAsync<DialogViewModel, string>("Navigated");
        await vm.WaitForViewClose();
        Console.WriteLine("Test Close");
    }
}
