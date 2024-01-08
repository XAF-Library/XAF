using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.Abstraction.Attributes;

public class StartupWindowAttribute : ViewForAttribute
{
    public StartupWindowAttribute(Type forType) : base(forType)
    {
    }
}

public class StartupWindowAttribute<T> : StartupWindowAttribute
    where T : StartupViewModel
{
    public StartupWindowAttribute()
        : base(typeof(T))
    {
    }
}
