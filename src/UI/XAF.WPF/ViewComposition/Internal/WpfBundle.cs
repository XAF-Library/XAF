using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.WPF.ViewComposition.Internal;
internal class WpfBundle<TViewModel> : IWpfBundle<TViewModel>
    where TViewModel : IXafViewModel
{
    public TViewModel ViewModel { get; }

    IXafViewModel IXafBundle.ViewModel => ViewModel;

    object IXafBundle.View => View;

    public FrameworkElement View { get; }

    public IBundleMetadata Metadata { get; }

    public WpfBundle(FrameworkElement view, TViewModel viewModel, IBundleMetadata metadata)
    {
        View = view;
        ViewModel = viewModel;
        Metadata = metadata;

        View.DataContext = ViewModel;
    }
}
