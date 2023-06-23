using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF;
using XAF.ReactiveProperty;

namespace WpfPlugin.ViewModels;
public class ViewBViewModel : NavigableViewModel<string>
{

    public RxProperty<string> Message { get; } = new("Default Message");

    public override void OnNavigatedTo(string parameter)
    {
        Message.Value = parameter;
    }
}
