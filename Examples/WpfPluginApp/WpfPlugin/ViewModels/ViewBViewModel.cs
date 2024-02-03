using System.ComponentModel;
using XAF.UI;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewModels;
using XAF.UI.ReactiveProperty;

namespace WpfPlugin.ViewModels;
public class ViewBViewModel : XafViewModel<string>
{
    public RxProperty<string> Message { get; } = new("Default Message");

    public override void Prepare(string parameter)
    {
        Message.Value = parameter;
    }
}
