using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;
using XAF.Utilities;

namespace XAF.UI.ViewModels;
public abstract class XafViewModel : NotifyPropertyChanged, IXafViewModel
{
    public virtual void Prepare() { }

    public virtual Task LoadAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task Unload()
    {
        return Task.CompletedTask;
    }
}

public abstract class XafViewModel<TParameter> : XafViewModel, IXafViewModel<TParameter>
{
    public abstract void Prepare(TParameter parameter);
}
