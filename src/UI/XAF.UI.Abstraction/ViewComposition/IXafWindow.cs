using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IXafWindow : IXafViewBundle
{
    
}

public interface IXafWindow<TViewModel> : IXafViewBundle<TViewModel>
    where TViewModel : IXafViewModel
{
}