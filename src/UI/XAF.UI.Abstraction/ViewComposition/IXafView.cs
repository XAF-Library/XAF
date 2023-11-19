using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IXafView
{
    IXafViewModel ViewModel { get; }
}

public interface IXafView<TViewModel> : IXafViewModel
    where TViewModel : IXafViewModel
{
    TViewModel ViewModel { get; }
}