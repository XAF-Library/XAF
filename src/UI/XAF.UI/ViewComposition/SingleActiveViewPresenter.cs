using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public class SingleActiveViewPresenter : ViewPresenter
{
    public SingleActiveViewPresenter(IViewAdapterCollection viewAdapters, IBundleProvider bundleProvider) 
        : base(viewAdapters, bundleProvider)
    {
    }

    public override void Activate(IXafBundle view)
    {
        var currentActive = ActiveViews.Items.FirstOrDefault();

        if (currentActive != null && currentActive != view && Views.Items.Contains(currentActive))
        {
            base.Deactivate(currentActive);
        }

        base.Activate(view);
    }

    public override void Add(IXafBundle view)
    {
        base.Add(view);
        if (ActiveViews.Count == 0)
        {
            Activate(view);
        }
    }
}
