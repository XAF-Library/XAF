using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction.Dialog;
public interface IInputDialogViewModel<out TResult> : IDialogViewModel
{
    new TResult? OnDialogClosed();
}

public interface IInputDialogViewModel<in TParameter, out TResult> : IDialogViewModel<TParameter>, IInputDialogViewModel<TResult>
{

}