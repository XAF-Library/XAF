using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.ViewModels;
public abstract class XafViewModel : IXafViewModel
{
    public abstract void Preload();
    
    public abstract Task Load();
}

public abstract class XafViewModel<TParameter> : IXafViewModel<TParameter>
{
    public abstract void Preload(TParameter parameter);
}
