using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public class AllActiveViewPresenter : ViewPresenter
{
    public AllActiveViewPresenter(IViewAdapterCollection viewAdapters, IBundleProvider bundleProvider) 
        : base(viewAdapters, bundleProvider)
    {
    }

    public override async Task AddAsync(IXafBundle view)
    {
        await base.AddAsync(view).ConfigureAwait(false);
        await base.ActivateAsync(view).ConfigureAwait(false);
    }

    public override async Task<bool> DeactivateAsync(IXafBundle view)
    {
        if (Views.Items.Contains(view))
        {
            await RemoveAsync(view).ConfigureAwait(false);
        }
        return await base.DeactivateAsync(view).ConfigureAwait(false);
    }

    public override Task ActivateAsync(IXafBundle view)
    {
        return AddAsync(view);
    }
}
