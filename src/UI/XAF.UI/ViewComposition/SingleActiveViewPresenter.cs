using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public abstract class SingleActiveViewPresenter<TView> : ViewPresenter<TView>
    where TView : class
{
    public override void Activate(IXafViewBundle view)
    {
        var currentActive = ActiveViews.Items.FirstOrDefault();

        if (currentActive != null && currentActive != view && Views.Items.Contains(currentActive))
        {
            base.Deactivate(currentActive);
        }

        base.Activate(view);
    }

    public override void Add(IXafViewBundle view)
    {
        base.Add(view);
        if (ActiveViews.Count == 0)
        {
            Activate(view);
        }
    }
}
