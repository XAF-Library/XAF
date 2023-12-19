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

    public override void Add(IXafBundle view)
    {
        base.Add(view);
        Activate(view);
    }

    public override void Activate(IXafBundle view)
    {
        throw new InvalidOperationException("Could not activate view in an all active view presenter");
    }

    public override bool Deactivate(IXafBundle view)
    {
        throw new InvalidOperationException("Could not deactivate view in an all active view presenter");
    }
}
