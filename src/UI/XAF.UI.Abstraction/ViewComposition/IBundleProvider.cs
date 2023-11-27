using DynamicData;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;

public interface IBundleProvider
{
    IEnumerable<IXafBundle<TViewModel>> GetBundles<TViewModel>()
        where TViewModel : IXafViewModel;

    IXafBundle<TViewModel> GetBundle<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    IXafBundle<TViewModel> GetOrCreateBundle<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    IXafBundle<TViewModel> GetOrCreateBundle<TViewModel>()
        where TViewModel : IXafViewModel;

    IXafBundle<TViewModel> CreateBundle<TViewModel>()
        where TViewModel : IXafViewModel;

    IXafBundle<TViewModel> CreateBundle<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;
}