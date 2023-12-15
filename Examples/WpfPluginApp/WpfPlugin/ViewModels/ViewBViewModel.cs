using System.ComponentModel;
using XAF.UI;
using XAF.UI.Abstraction;
using XAF.UI.ReactiveProperty;
using XAF.UI.ViewModels;

namespace WpfPlugin.ViewModels;
public class ViewBViewModel : XafViewModel<string>
{
    public RxProperty<string> Message { get; } = new("Default Message");

    public override void Preload(string parameter)
    {
        Message.Value = parameter;
    }
}
