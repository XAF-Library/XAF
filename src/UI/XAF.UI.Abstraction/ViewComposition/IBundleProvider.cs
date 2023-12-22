using DynamicData;
using System.Diagnostics.CodeAnalysis;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;

public interface IBundleProvider
{
    void AddBundle(IXafBundle bundle);

    IEnumerable<IXafBundle<TViewModel>> GetBundles<TViewModel>()
        where TViewModel : IXafViewModel;

    bool TryGetFirstBundle<TViewModel>([NotNullWhen(true)] out IXafBundle<TViewModel>? bundle)
        where TViewModel : IXafViewModel;

    bool TryGetBundle<TViewModel>(TViewModel viewModel, [NotNullWhen(true)] out IXafBundle<TViewModel>? bundle)
        where TViewModel : IXafViewModel;

    ValueTask<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    ValueTask<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    IAsyncEnumerable<IXafBundle> CreateBundlesWithDecoratorAsync<TViewDecorator>()
        where TViewDecorator : BundleDecoratorAttribute;

    Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;
}