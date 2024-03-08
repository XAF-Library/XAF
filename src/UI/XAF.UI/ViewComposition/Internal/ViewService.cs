using DynamicData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.ViewComposition.Internal;
internal class ViewService : IViewService
{
    private readonly Dictionary<object, IViewPresenter> _viewPresenters = new();

    public void AddPresenter(IViewPresenter presenter, object key)
    {
        _viewPresenters.Add(key, presenter);
    }

    public IViewPresenter GetPresenter(object key)
    {
        return _viewPresenters[key];
    }

    public bool ContainsPresenter(object key)
    {
        return _viewPresenters.ContainsKey(key);
    }

    public async Task<IEnumerable<IXafBundle<TViewModel>>> ActivateViewsAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {

        var viewPresenter = _viewPresenters[key];

        var bundles = viewPresenter.BundleProvider.GetBundles<TViewModel>();

        if (bundles.Any())
        {
            foreach (var bundle in bundles)
            {
                bundle.ViewModel.Prepare();
                await viewPresenter.ActivateAsync(bundle);
            }

            return bundles;
        }

        var newBundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        newBundle.ViewModel.Prepare();
        await viewPresenter.ActivateAsync(newBundle);
        return Enumerable.Repeat(newBundle, 1);
    }

    public async Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {

        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync(viewModel).ConfigureAwait(false);
        bundle.ViewModel.Prepare();
        await viewPresenter.ActivateAsync(bundle);
        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, TViewModel viewModel, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync(viewModel).ConfigureAwait(false);
        viewModel.Prepare();
        viewModel.Prepare(parameter);
        await viewPresenter.ActivateAsync(bundle);
        return bundle;
    }

    public async Task<IXafBundle> ActivateViewAsync(Type viewModelType, object key)
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync(viewModelType)
            .ConfigureAwait(false);

        await ActivateViewAsync(bundle, key);
        return bundle;
    }

    public async Task<IXafBundle> ActivateViewAsync(Type viewModelType, object parameter, object key)
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync(viewModelType)
            .ConfigureAwait(false);

        await ActivateViewAsync(bundle, parameter, key);
        return bundle;
    }

    public async Task ActivateViewAsync(IXafBundle bundle, object key)
    {
        var viewPresenter = _viewPresenters[key];
        var viewModel = bundle.ViewModel;
        viewModel.Prepare();
        await viewPresenter.ActivateAsync(bundle);
    }

    public async Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync<TViewModel>().ConfigureAwait(false);
        bundle.ViewModel.Prepare();
        bundle.ViewModel.Prepare(parameter);
        await viewPresenter.ActivateAsync(bundle);
        return bundle;
    }

    public async Task ActivateViewAsync(IXafBundle bundle, object parameter, object key)
    {
        var viewPresenter = _viewPresenters[key];
        bundle.ViewModel.Prepare();

        if (parameter.GetType().IsAssignableTo(bundle.Metadata.ParameterType))
        {
            var prepareMethod = bundle.Metadata.ViewModelType.GetMethods()
                .Single(m => m.Name == nameof(IXafViewModel.Prepare) && m.GetParameters().Length == 1);

            prepareMethod.Invoke(bundle.ViewModel, new[] { parameter });
        }

        await viewPresenter.ActivateAsync(bundle);
    }

    public async Task<IEnumerable<IXafBundle<TViewModel>>> ActivateViewsAsync<TViewModel, TParameter>(TParameter parameter, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];

        var bundles = viewPresenter.BundleProvider.GetBundles<TViewModel>();

        if (bundles.Any())
        {
            foreach (var bundle in bundles)
            {
                bundle.ViewModel.Prepare();
                bundle.ViewModel.Prepare(parameter);
                await viewPresenter.ActivateAsync(bundle);
            }

            return bundles;
        }

        var newBundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        newBundle.ViewModel.Prepare();
        newBundle.ViewModel.Prepare(parameter);
        await viewPresenter.ActivateAsync(newBundle);
        return Enumerable.Repeat(newBundle, 1);
    }

    public async Task<IXafBundle<TViewModel>> ActivateFirstViewAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        if (!viewPresenter.BundleProvider.TryGetFirstBundle<TViewModel>(out var bundle))
        {
            bundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        }

        bundle.ViewModel.Prepare();
        await viewPresenter.ActivateAsync(bundle);
        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> ActivateFirstViewAsync<TViewModel, TParameter>(TParameter parameter, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];

        if (!viewPresenter.BundleProvider.TryGetFirstBundle<TViewModel>(out var bundle))
        {
            bundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        }

        bundle.ViewModel.Prepare();
        bundle.ViewModel.Prepare(parameter);
        await viewPresenter.ActivateAsync(bundle);
        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> AddViewAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        await viewPresenter.AddAsync(bundle);
        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> AddViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.CreateBundleAsync(viewModel).ConfigureAwait(false);
        await viewPresenter.AddAsync(bundle);
        return bundle;
    }

    public async Task<IXafBundle> AddViewAsync(Type viewModelType, object key)
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.CreateBundleAsync(viewModelType)
            .ConfigureAwait(false);
        await viewPresenter.AddAsync(bundle);
        return bundle;
    }

    public Task AddViewAsync(IXafBundle bundle, object key)
    {
        var viewPresenter = _viewPresenters[key];
        viewPresenter.AddAsync(bundle);
        return Task.CompletedTask;
    }

    public async Task DeactivateViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        if (viewPresenter.BundleProvider.TryGetBundle(viewModel, out var bundle))
        {
            await viewPresenter.DeactivateAsync(bundle);
            await bundle.ViewModel.UnloadAsync().ConfigureAwait(false);
        }
    }

    public async Task DeactivateViewsAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = viewPresenter.BundleProvider.GetBundles<TViewModel>();
        foreach (var bundle in bundles)
        {
            await viewPresenter.DeactivateAsync(bundle);
        }
    }

    public async Task DeactivateViewsAsync(Type viewModelType, object key)
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = viewPresenter.BundleProvider.GetBundles(viewModelType);
        foreach (var bundle in bundles)
        {
            await viewPresenter.DeactivateAsync(bundle);
        }
    }

    public Task DeactivateViewAsync(IXafBundle bundle, object key)
    {
        var viewPresenter = _viewPresenters[key];

        return viewPresenter.DeactivateAsync(bundle);
    }

    public async Task RemoveViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        if (viewPresenter.BundleProvider.TryGetBundle(viewModel, out var bundle))
        {
            await viewPresenter.RemoveAsync(bundle);
        }
    }

    public async Task RemoveViewsAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = viewPresenter.BundleProvider.GetBundles<TViewModel>();
        foreach (var bundle in bundles)
        {
            await viewPresenter.RemoveAsync(bundle);
        }
    }

    public async Task RemoveViewsAsync(Type viewModelType, object key)
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = viewPresenter.BundleProvider.GetBundles(viewModelType);
        foreach (var bundle in bundles)
        {
            await viewPresenter.RemoveAsync(bundle);
        }
    }

    public async Task RemoveViewAsync(IXafBundle bundle, object key)
    {
        var viewPresenter = _viewPresenters[key];
        await viewPresenter.RemoveAsync(bundle);
    }

    public async Task RemoveAllAsync(object key)
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = viewPresenter.BundleProvider.GetBundles();

        foreach (var bundle in bundles)
        {
            await viewPresenter.RemoveAsync(bundle);
        }
    }
}
