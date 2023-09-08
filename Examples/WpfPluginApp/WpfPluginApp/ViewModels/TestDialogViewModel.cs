using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Dialog;

namespace WpfPluginApp.ViewModels;
public class TestDialogViewModel : InputDialogViewModel<string,string>
{
    public override string Title => "Test";

    public override string? OnDialogClosed()
    {
        return "Success";
    }

    public override void OnDialogOpened(string input)
    {
        var testInput = input;
    }
}
