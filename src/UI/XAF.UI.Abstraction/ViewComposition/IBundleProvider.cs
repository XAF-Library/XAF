using DynamicData;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;

public interface IBundleProvider
{
    Task<IEnumerable<IXafBundle<TViewModel>>> GetBundlesAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> GetBundleAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task<IXafBundle> CreateBundleWithDecoratorAsync<TViewDecorator>()
        where TViewDecorator : BundleDecoratorAttribute;

    Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;
}