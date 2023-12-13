using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;
using XAF.UI.WPF.Hosting;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.ViewComposition.Internal;
internal class BundleProvider : IBundleProvider
{
    private readonly IBundleMetadataCollection _bundleMetadata;
    private readonly IServiceProvider _serviceProvider;
    private readonly IWpfThread _wpfThread;
    private readonly Dictionary<IXafViewModel, IXafBundle> _bundleByViewModel;
    private readonly Dictionary<Type, List<IXafBundle>> _bundlesByViewModelTypes;

    public BundleProvider(IBundleMetadataCollection bundleMetadata, IServiceProvider serviceProvider, IWpfThread wpfThread)
    {
        _bundleMetadata = bundleMetadata;
        _serviceProvider = serviceProvider;
        _wpfThread = wpfThread;
    }

    public async Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        var metadata = _bundleMetadata.GetMetadataForViewModel<TViewModel>();
        var view = await CreateViewAsync(metadata.ViewType).ConfigureAwait(false);
        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        var bundle = new WpfBundle<TViewModel>(view, vm, metadata);

        _bundlesByViewModelTypes.Add(typeof(TViewModel), (IXafBundle)bundle);
        _bundleByViewModel.Add(vm, bundle);

        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        var metadata = _bundleMetadata.GetMetadataForViewModel<TViewModel>();
        var view = await CreateViewAsync(metadata.ViewType).ConfigureAwait(false);
        var bundle = new WpfBundle<TViewModel>(view, viewModel, metadata);

        _bundlesByViewModelTypes.Add(typeof(TViewModel), (IXafBundle)bundle);
        _bundleByViewModel.Add(viewModel, bundle);

        return bundle;
    }

    public async Task<IXafBundle> CreateBundleWithDecoratorAsync<TViewDecorator>() where TViewDecorator : BundleDecoratorAttribute
    {
        var metadata = _bundleMetadata.GetMetadataForDecorator<TViewDecorator>().First();
        var view = await CreateViewAsync(metadata.ViewType).ConfigureAwait(false);
        var vm = (IXafViewModel)_serviceProvider.GetRequiredService(metadata.ViewModelType);
        var bundle = new WpfBundle(view, vm, metadata);

        _bundlesByViewModelTypes.Add(vm.GetType(), (IXafBundle)bundle);
        _bundleByViewModel.Add(vm, bundle);

        return bundle;
    }

    public Task<IXafBundle<TViewModel>> GetBundleAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        return Task.FromResult((IXafBundle<TViewModel>)_bundleByViewModel[viewModel]);
    }

    public Task<IEnumerable<IXafBundle<TViewModel>>> GetBundlesAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        return !_bundlesByViewModelTypes.ContainsKey(typeof(TViewModel))
            ? Task.FromResult(Enumerable.Empty<IXafBundle<TViewModel>>())
            : Task.FromResult(_bundlesByViewModelTypes[typeof(TViewModel)].OfType<IXafBundle<TViewModel>>());
    }

    public Task<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        return _bundleByViewModel.TryGetValue(viewModel, out var bundle)
            ? Task.FromResult((IXafBundle<TViewModel>)bundle)
            : CreateBundleAsync(viewModel);
    }

    public Task<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        if (_bundlesByViewModelTypes.TryGetValue(typeof(TViewModel), out var bundles))
        {
            var bundle = bundles.FirstOrDefault();
            if (bundle != null)
            {
                return Task.FromResult((IXafBundle<TViewModel>)bundles.First());
            }
        }
        return CreateBundleAsync<TViewModel>();
    }

    private async Task<FrameworkElement> CreateViewAsync(Type ViewType)
    {
        await _wpfThread.WaitForAppCreation();
        return await _wpfThread.UiDispatcher.InvokeAsync(() => (FrameworkElement)_serviceProvider.GetRequiredService(ViewType));
    }
}
