using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.WPF.ViewComposition.Internal;
internal class BundleProvider : IBundleProvider
{
    private readonly IBundleMetadataCollection _bundleMetadata;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<IXafViewModel, IXafBundle> _bundleByViewModel;
    private readonly Dictionary<Type, List<IXafBundle>> _bundlesByViewModelTypes;

    public BundleProvider(IBundleMetadataCollection bundleMetadata, IServiceProvider serviceProvider)
    {
        _bundleMetadata = bundleMetadata;
        _serviceProvider = serviceProvider;
    }

    public IXafBundle<TViewModel> CreateBundle<TViewModel>() where TViewModel : IXafViewModel
    {
        var metadata = _bundleMetadata.GetMetadataForViewModel<TViewModel>();
        var view = (FrameworkElement)_serviceProvider.GetRequiredService(metadata.ViewType);
        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        var bundle = new WpfBundle<TViewModel>(view, vm, metadata);

        _bundlesByViewModelTypes.Add(typeof(TViewModel), (IXafBundle)bundle);
        _bundleByViewModel.Add(vm, bundle);

        return bundle;
    }

    public IXafBundle<TViewModel> CreateBundle<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        var metadata = _bundleMetadata.GetMetadataForViewModel<TViewModel>();
        var view = (FrameworkElement)_serviceProvider.GetRequiredService(metadata.ViewType);
        var bundle = new WpfBundle<TViewModel>(view, viewModel, metadata);

        _bundlesByViewModelTypes.Add(typeof(TViewModel), (IXafBundle)bundle);
        _bundleByViewModel.Add(viewModel, bundle);

        return bundle;
    }

    public IXafBundle<TViewModel> GetBundle<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        return (IXafBundle<TViewModel>)_bundleByViewModel[viewModel];
    }

    public IEnumerable<IXafBundle<TViewModel>> GetBundles<TViewModel>() where TViewModel : IXafViewModel
    {
        if (!_bundlesByViewModelTypes.ContainsKey(typeof(TViewModel)))
        {
            return Enumerable.Empty<IXafBundle<TViewModel>>();
        }

        return _bundlesByViewModelTypes[typeof(TViewModel)].OfType<IXafBundle<TViewModel>>();
    }

    public IXafBundle<TViewModel> GetOrCreateBundle<TViewModel>(TViewModel viewModel) where TViewModel : IXafViewModel
    {
        return _bundleByViewModel.TryGetValue(viewModel, out var bundle)
            ? (IXafBundle<TViewModel>)bundle
            : CreateBundle(viewModel);
    }

    public IXafBundle<TViewModel> GetOrCreateBundle<TViewModel>() where TViewModel : IXafViewModel
    {
        if (_bundlesByViewModelTypes.TryGetValue(typeof(TViewModel), out var bundles))
        {
            var bundle = bundles.FirstOrDefault();
            if (bundle != null)
            {
                return (IXafBundle<TViewModel>)bundles.First();
            }
        }
        return CreateBundle<TViewModel>();
    }
}
