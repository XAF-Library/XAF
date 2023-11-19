using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IXafWindow : IXafView
{
    
}

public interface IXafWindow<TViewModel> : IXafView<TViewModel>
    where TViewModel : IXafViewModel
{
}