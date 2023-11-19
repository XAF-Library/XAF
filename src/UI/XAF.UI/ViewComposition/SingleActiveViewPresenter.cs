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
    protected SingleActiveViewPresenter(object key, TView targetView) : base(key, targetView)
    {
    }

    public override void Activate(IXafView view)
    {
        var currentActive = ActiveViews.Items.FirstOrDefault();

        if (currentActive != null && currentActive != view && Views.Items.Contains(currentActive))
        {
            base.Deactivate(currentActive);
        }

        base.Activate(view);
    }
}
