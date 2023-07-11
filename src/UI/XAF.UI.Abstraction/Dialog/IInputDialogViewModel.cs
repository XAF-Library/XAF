using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction.Dialog;
public interface IInputDialogViewModel<out TResult> : IViewModel
{
    string Title { get; }
    void OnDialogOpened();
    TResult? OnDialogClosed();
}

public interface IInputDialogViewModel<in TParameter, out TResult> : IViewModel
{
    string Title { get; }
    void OnDialogOpened(TParameter parameter);
    TResult? OnDialogClosed();
}