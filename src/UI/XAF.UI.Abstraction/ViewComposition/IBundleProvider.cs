using DynamicData;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;

public interface IBundleProvider
{
    void AddBundle(IXafBundle bundle);

    Task<IEnumerable<IXafBundle<TViewModel>>> GetBundlesAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>?> GetFirstBundleAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> GetBundleAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    IAsyncEnumerable<IXafBundle> CreateBundlesWithDecoratorAsync<TViewDecorator>()
        where TViewDecorator : BundleDecoratorAttribute;

    Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;
}