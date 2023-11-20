using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public abstract class AllActiveViewPresenter<TView> : ViewPresenter<TView>
    where TView : class
{
    public override void Add(IXafViewBundle view)
    {
        base.Add(view);
        Activate(view);
    }

    public override void Activate(IXafViewBundle view)
    {
        throw new InvalidOperationException("Could not activate view in an all active view presenter");
    }

    public override void Deactivate(IXafViewBundle view)
    {
        throw new InvalidOperationException("Could not deactivate view in an all active view presenter");
    }
}
