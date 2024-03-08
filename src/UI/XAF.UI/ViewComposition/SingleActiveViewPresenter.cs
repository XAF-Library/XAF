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

    public override async Task ActivateAsync(IXafBundle view)
    {
        var currentActive = ActiveViews.Items.FirstOrDefault();

        if (currentActive != null && currentActive != view && Views.Items.Contains(currentActive))
        {
            await base.DeactivateAsync(currentActive);
        }

        await base.ActivateAsync(view);
    }

    public override async Task AddAsync(IXafBundle view)
    {
        await base.AddAsync(view);
        if (ActiveViews.Count == 0)
        {
            await ActivateAsync(view);
        }
    }
}
