using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.ViewModels;
public abstract class XafViewModel : IXafViewModel
{
    public abstract void PreInitialize();
    
    public abstract Task Initialize();
}

public abstract class XafViewModel<TParameter> : IXafViewModel<TParameter>
{
    public abstract void PreInitialize(TParameter parameter);
}
