using XAF.UI;
using XAF.UI.Reactive.ReactiveProperty;

namespace WpfPlugin.ViewModels;
public class ViewBViewModel : NavigableViewModel<string>
{
    public RxProperty<string> Message { get; } = new("Default Message");

    public override void OnNavigatedTo(string parameter)
    {
        Message.Value = parameter;
    }
}
