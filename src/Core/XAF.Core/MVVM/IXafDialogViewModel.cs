using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Core.MVVM;
public interface IXafDialogViewModel<TResult> : IXafViewModel
{
    event EventHandler<TResult?> RequestClose;

    bool CanClose();
}

public interface IXafDialogViewModel<TResult, TParameter> : IXafViewModel<TParameter>, IXafDialogViewModel<TResult>
{

}
