using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace WpfPluginApp.ViewModels;
internal class DialogViewModel : XafViewModel<string>
{
    private string _message;

    private string _displayMessage;

    public string DisplayMessage
    {
        get => _displayMessage;
        set => SetProperty(ref _displayMessage, value);
    }

    public override void Prepare(string parameter)
    {
        _message = parameter;
        DisplayMessage = "Loading";
    }

    public override async Task LoadAsync()
    {
        await Task.Delay(2000);
        DisplayMessage = _message;
    }
}
