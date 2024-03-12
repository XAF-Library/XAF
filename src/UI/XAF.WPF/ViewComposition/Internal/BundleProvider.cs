using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
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
    private readonly IWpfEnvironment _wpfEnvironment;
    private readonly Dictionary<IXafViewModel, IXafBundle> _bundleByViewModel;
    private readonly Dictionary<Type, List<IXafBundle>> _bundlesByViewModelTypes;

    public BundleProvider(IBundleMetadataCollection bundleMetadata, IServiceProvider serviceProvider, IWpfEnvironment wpfEnvironment)
    {
        _bundleMetadata = bundleMetadata;
        _serviceProvider = serviceProvider;
        _wpfEnvironment = wpfEnvironment;
        _bundlesByViewModelTypes = new();
        _bundleByViewModel = new();
    }

    public bool CacheBundles { get; set; } = true;

    public void AddBundle(IXafBundle bundle)
    {
        if (_bundleByViewModel.ContainsKey(bundle.ViewModel))
        {
            return;
        }

        if (CacheBundles)
        {
            _bundleByViewModel.Add(bundle.ViewModel, bundle);
            _bundlesByViewModelTypes.Add(bundle.ViewModelType, bundle);
        }
    }

    public async Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        var metadata = _bundleMetadata.GetMetadataForViewModel<TViewModel>();
        var vm = (TViewModel)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, metadata.ViewModelType);
        var view = await CreateViewAsync(metadata.ViewType, vm).ConfigureAwait(false);
        var bundle = new WpfBundle<TViewModel>(view, vm, metadata);



        if (CacheBundles)
        {
            _bundlesByViewModelTypes.Add(typeof(TViewModel), (IXafBundle)bundle);
            _bundleByViewModel.Add(vm, bundle); 
        }

        return bundle;
    }

    public async Task<IXafBundle> CreateBundleAsync(Type viewModelType)
    {
        var metadata = _bundleMetadata.GetMetadataForViewModel(viewModelType);
        var vm = (IXafViewModel)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, metadata.ViewModelType);
        var view = await CreateViewAsync(metadata.ViewType, vm).ConfigureAwait(false);
        var bundle = new WpfBundle(view, vm, metadata);

        if (CacheBundles)
        {
            _bundlesByViewModelTypes.Add(viewModelType, (IXafBundle)bundle);
            _bundleByViewModel.Add(vm, bundle); 
        }

        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> CreateBundleAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        var metadata = _bundleMetadata.GetMetadataForViewModel(viewModel.GetType());
        var view = await CreateViewAsync(metadata.ViewType, viewModel).ConfigureAwait(false);
        var bundle = new WpfBundle<TViewModel>(view, viewModel, metadata);

        if (CacheBundles)
        {
            _bundlesByViewModelTypes.Add(typeof(TViewModel), (IXafBundle)bundle);
            _bundleByViewModel.Add(viewModel, bundle); 
        }

        return bundle;
    }

    public async IAsyncEnumerable<IXafBundle> CreateBundlesWithDecoratorAsync<TViewDecorator>() where TViewDecorator : BundleDecoratorAttribute
    {
        var metadata = _bundleMetadata.GetMetadataForDecorator<TViewDecorator>();
        foreach (var item in metadata)
        {
            var vm = (IXafViewModel)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, item.ViewModelType);
            var view = await CreateViewAsync(item.ViewType, vm).ConfigureAwait(false);
            var bundle = new WpfBundle(view, vm, item);


            if (CacheBundles)
            {
                _bundlesByViewModelTypes.Add(vm.GetType(), (IXafBundle)bundle);
                _bundleByViewModel.Add(vm, bundle);
            }
            
            yield return bundle;
        }
    }

    public bool TryGetBundle<TViewModel>(TViewModel viewModel, [NotNullWhen(true)] out IXafBundle<TViewModel>? bundle) where TViewModel : IXafViewModel
    {
        bundle = null;
        if (_bundleByViewModel.TryGetValue(viewModel, out var potentialBundle) &&
            potentialBundle is IXafBundle<TViewModel> resultBundle)
        {
            bundle = resultBundle;
            return true;
        }
        return false;
    }

    public IEnumerable<IXafBundle<TViewModel>> GetBundles<TViewModel>() where TViewModel : IXafViewModel
    {
        return _bundlesByViewModelTypes.GetOrEmpty<Type, List<IXafBundle>, IXafBundle>(typeof(TViewModel))
            .OfType<IXafBundle<TViewModel>>();
    }

    public IEnumerable<IXafBundle> GetBundles(Type viewModelType)
    {
        return _bundlesByViewModelTypes.GetOrEmpty<Type, List<IXafBundle>, IXafBundle>(viewModelType);
    }

    public IEnumerable<IXafBundle> GetBundles()
    {
        return _bundlesByViewModelTypes.Values.SelectMany(b => b);
    }

    public bool TryGetFirstBundle<TViewModel>(out IXafBundle<TViewModel>? bundle) where TViewModel : IXafViewModel
    {
        bundle = null;

        if (!_bundlesByViewModelTypes.TryGetValue(typeof(TViewModel), out var bundles))
        {
            return false;
        }

        bundle = bundles.OfType<IXafBundle<TViewModel>>().FirstOrDefault();
        return bundle is not null;
    }

    public bool TryGetFirstBundle(Type viewModelType, [NotNullWhen(true)] out IXafBundle? bundle)
    {
        bundle = null;

        if (!_bundlesByViewModelTypes.TryGetValue(viewModelType, out var bundles))
        {
            return false;
        }

        bundle = bundles.FirstOrDefault();
        return bundle is not null;
    }

    public ValueTask<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        return _bundleByViewModel.TryGetValue(viewModel, out var bundle)
            ? new ValueTask<IXafBundle<TViewModel>>((IXafBundle<TViewModel>)bundle)
            : new ValueTask<IXafBundle<TViewModel>>(CreateBundleAsync(viewModel));
    }

    public ValueTask<IXafBundle<TViewModel>> GetOrCreateBundleAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        if (_bundlesByViewModelTypes.TryGetValue(typeof(TViewModel), out var bundles))
        {
            var bundle = bundles.FirstOrDefault();
            if (bundle != null)
            {
                return new ValueTask<IXafBundle<TViewModel>>((IXafBundle<TViewModel>)bundle);
            }
        }
        return new ValueTask<IXafBundle<TViewModel>>(CreateBundleAsync<TViewModel>());
    }

    public ValueTask<IXafBundle> GetOrCreateBundleAsync(Type viewModelType)
    {
        if (_bundlesByViewModelTypes.TryGetValue(viewModelType, out var bundles))
        {
            var bundle = bundles.FirstOrDefault();
            if (bundle != null)
            {
                return new ValueTask<IXafBundle>(bundle);
            }
        }
        return new ValueTask<IXafBundle>(CreateBundleAsync(viewModelType));
    }

    private async Task<FrameworkElement> CreateViewAsync(Type ViewType, IXafViewModel viewModel)
    {
        await _wpfEnvironment.WaitForAppStart();
        return _wpfEnvironment.WpfDispatcher.Invoke(() =>
        {
            var view = (FrameworkElement)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, ViewType);
            view.DataContext = viewModel;
            return view;
        });
    }
}
