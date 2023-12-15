using System.Windows;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.WPF.ViewComposition.Internal;

internal class WpfBundle : IWpfBundle
{
    public IXafViewModel ViewModel { get; }

    object IXafBundle.View => View;

    public FrameworkElement View { get; }

    public IBundleMetadata Metadata { get; }

    public WpfBundle(FrameworkElement view, IXafViewModel viewModel, IBundleMetadata metadata)
    {
        View = view;
        ViewModel = viewModel;
        Metadata = metadata;
    }
}

internal class WpfBundle<TViewModel> : WpfBundle,IWpfBundle<TViewModel>
    where TViewModel : IXafViewModel
{
    new public TViewModel ViewModel { get; }

    public WpfBundle(FrameworkElement view, TViewModel viewModel, IBundleMetadata metadata)
        : base(view, viewModel, metadata)
    {
        ViewModel = viewModel;
    }
}
