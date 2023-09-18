using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Dialog;

namespace WpfPluginApp.ViewModels;
public class TestDialogViewModel : ViewModelBase, IResultViewModel<string, string>, IDialogProperties
{
    private string _title = "Test";

    public string Title 
    {
        get => _title;
        protected set => Set(ref _title, value); 
    }

    public string? CreateResult()
    {
        return "Success";
    }

    public void OnActivated(string input)
    {
        var testInput = input;
    }

    public void OnDeactivated()
    {
    }
}
