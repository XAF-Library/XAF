using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IXafViewBundle
{
    IViewMetadata ViewMetadata { get; }

    IXafViewModel ViewModel { get; }

    object View { get; }
}

public interface IXafViewBundle<TViewModel> : IXafViewBundle
    where TViewModel : IXafViewModel
{
    TViewModel ViewModel { get; }
}