using System.Reflection.Metadata;
using System.Windows.Input;
using XAF.UI.Abstraction.Commands;

namespace XAF.UI.Abstraction.Dialog;
public interface IDialogViewModel : IViewModel
{
    string Title { get; }
    
    void OnDialogOpened();
    void OnDialogClosed();
}

public interface IDialogViewModel<in TParameter> : IDialogViewModel
{
    void OnDialogOpend(TParameter parameter);
}
