using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.WPF.ViewComposition;
internal interface IWpfBundle<TViewModel> : IXafBundle<TViewModel>
    where TViewModel : IXafViewModel
{
    new public FrameworkElement View { get; }
}

internal interface IWpfBundle : IXafBundle
{
    new public FrameworkElement View { get; }
}
