using System.ComponentModel;
using XAF.UI;
using XAF.UI.Abstraction;
using XAF.UI.Reactive.ReactiveProperty;

namespace WpfPlugin.ViewModels;
public class ViewBViewModel : IActivatableViewModel<string>
{
    public RxProperty<string> Message { get; } = new("Default Message");

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnDeactivated()
    {
    }

    public void OnActivated(string parameter)
    {
        Message.Value = parameter;
    }
}
