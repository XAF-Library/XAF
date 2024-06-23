using System.Windows;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.WPF.ViewComposition.Internal;

internal class WpfBundle : IWpfBundle
{
    public IXafViewModel ViewModel { get; }

    object IXafBundle.View => View;

    public FrameworkElement View { get; }

    public Type ViewModelType { get; }
    public Type ViewType { get; }
    public Type? ParameterType { get; }
    public IBundleDecoratorCollection ViewDecorators { get; }

    public WpfBundle(FrameworkElement view, IXafViewModel viewModel, IBundleMetadata metadata)
    {
        View = view;
        ViewModel = viewModel;
        ViewModelType = metadata.ViewModelType;
        ViewType = metadata.ViewType;
        ParameterType = metadata.ParameterType;
        ViewDecorators = metadata.ViewDecorators;
    }

    public int CompareTo(IXafBundle? other)
    {
        if (other != null)
        {
            return ViewModel.CompareTo(other.ViewModel);
        }

        return 1;
    }
}

internal class WpfBundle<TViewModel> : WpfBundle, IWpfBundle<TViewModel>
    where TViewModel : IXafViewModel
{
    new public TViewModel ViewModel { get; }

    public WpfBundle(FrameworkElement view, TViewModel viewModel, IBundleMetadata metadata)
        : base(view, viewModel, metadata)
    {
        ViewModel = viewModel;
    }
}
