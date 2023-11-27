using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IXafBundle
{
    IXafViewModel ViewModel { get; }

    object View { get; }

    IBundleMetadata Metadata { get; }
}

public interface IXafBundle<TViewModel> : IXafBundle
    where TViewModel : IXafViewModel 
{
    new TViewModel ViewModel { get; }
}