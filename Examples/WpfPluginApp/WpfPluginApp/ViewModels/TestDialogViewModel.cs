using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Dialog;

namespace WpfPluginApp.ViewModels;
public class TestDialogViewModel : DialogViewModel
{
    public override string Title { get; protected set; }

    public TestDialogViewModel()
    {
        Title = "Test";
    }

    public override void OnDialogOpened()
    {
        base.OnDialogOpened();
    }

    public override void OnDialogClosed()
    {
        base.OnDialogClosed();
    }
}
