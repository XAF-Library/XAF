using System.Windows.Input;

namespace XAF.UI.Abstraction.Dialog;

public class DialogButton
{
    public object Content { get; set; }

    public ICommand Command { get; set; }


    public DialogButton(object content, ICommand command)
    {
        Content = content;
        Command = command;
    }
}