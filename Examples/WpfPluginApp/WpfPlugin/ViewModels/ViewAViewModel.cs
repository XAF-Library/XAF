using System.Reactive.Linq;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.ReactiveCommands;
using XAF.UI.ReactiveProperty;
using XAF.UI.ViewModels;

namespace WpfPlugin.ViewModels;
public class ViewAViewModel : XafViewModel
{
    public RxProperty<string> Message { get; } = new();

    public RxCommand NavigateToViewBCommand { get; }

    public ViewAViewModel(INavigationService navigationService)
    {
        // Executes navigation to View B. 
        // Can only be executed if Message is not empty.
        NavigateToViewBCommand = RxCommand.CreateFromTask(
            () => navigationService.NavigateAsync<ViewBViewModel, string>(Message, "PageViews"),
            Message.Select(s => !string.IsNullOrWhiteSpace(s)));
    }

    public override void Prepare()
    {
        base.Prepare();
        Message.Value = string.Empty;
    }
}
