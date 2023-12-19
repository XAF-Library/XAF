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

        var bundles = await viewPresenter.BundleProvider.GetBundlesAsync<TViewModel>().ConfigureAwait(false);

        if (bundles.Any())
        {
            foreach (var bundle in bundles)
            {
                bundle.ViewModel.Prepare();
                viewPresenter.Activate(bundle);
                await bundle.ViewModel.LoadAsync().ConfigureAwait(false);
            }

            return bundles;
        }

        var newBundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        newBundle.ViewModel.Prepare();
        viewPresenter.Activate(newBundle);
        await newBundle.ViewModel.LoadAsync().ConfigureAwait(false);
        return Enumerable.Repeat(newBundle, 1);
    }

    public async Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {

        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync(viewModel).ConfigureAwait(false);
        bundle.ViewModel.Prepare();
        viewPresenter.Activate(bundle);
        await viewModel.LoadAsync().ConfigureAwait(false);
        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, TViewModel viewModel, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync(viewModel).ConfigureAwait(false);
        viewModel.Prepare();
        viewModel.Prepare(parameter);
        viewPresenter.Activate(bundle);
        await viewModel.LoadAsync().ConfigureAwait(false);
        return bundle;
    }

    public async Task ActivateViewAsync(IXafBundle bundle, object key)
    {
        var viewPresenter = _viewPresenters[key];
        var viewModel = bundle.ViewModel;
        viewModel.Prepare();
        viewPresenter.Activate(bundle);
        await viewModel.LoadAsync().ConfigureAwait(false);
    }

    public async Task<IXafBundle<TViewModel>> ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync<TViewModel>().ConfigureAwait(false);
        bundle.ViewModel.Prepare();
        bundle.ViewModel.Prepare(parameter);
        viewPresenter.Activate(bundle);
        await bundle.ViewModel.LoadAsync().ConfigureAwait(false);
        return bundle;
    }

    public async Task ActivateViewAsync(IXafBundle bundle, object parameter, object key)
    {
        var viewPresenter = _viewPresenters[key];
        bundle.ViewModel.Prepare();

        if (bundle.Metadata.ParameterType == parameter.GetType())
        {
            var prepareMethod = bundle.Metadata.ViewModelType.GetMethods()
                .Single(m => m.Name == nameof(IXafViewModel.Prepare) && m.GetParameters().Length == 1);

            prepareMethod = prepareMethod.MakeGenericMethod(parameter.GetType());
            prepareMethod.Invoke(bundle.View, new[] { parameter });
        }

        viewPresenter.Activate(bundle);
        await bundle.ViewModel.LoadAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<IXafBundle<TViewModel>>> ActivateViewsAsync<TViewModel, TParameter>(TParameter parameter, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];

        var bundles = await viewPresenter.BundleProvider.GetBundlesAsync<TViewModel>().ConfigureAwait(false);

        if (bundles.Any())
        {
            foreach (var bundle in bundles)
            {
                bundle.ViewModel.Prepare();
                bundle.ViewModel.Prepare(parameter);
                viewPresenter.Activate(bundle);
                await bundle.ViewModel.LoadAsync().ConfigureAwait(false);
            }

            return bundles;
        }

        var newBundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        newBundle.ViewModel.Prepare();
        newBundle.ViewModel.Prepare(parameter);
        viewPresenter.Activate(newBundle);
        await newBundle.ViewModel.LoadAsync().ConfigureAwait(false);
        return Enumerable.Repeat(newBundle, 1);
    }

    public async Task<IXafBundle<TViewModel>> ActivateFirstViewAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.GetFirstBundleAsync<TViewModel>().ConfigureAwait(false);

        bundle ??= await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);

        bundle.ViewModel.Prepare();
        viewPresenter.Activate(bundle);
        await bundle.ViewModel.LoadAsync().ConfigureAwait(false);
        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> ActivateFirstViewAsync<TViewModel, TParameter>(TParameter parameter, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.GetFirstBundleAsync<TViewModel>().ConfigureAwait(false);

        bundle ??= await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);

        bundle.ViewModel.Prepare();
        bundle.ViewModel.Prepare(parameter);
        viewPresenter.Activate(bundle);
        await bundle.ViewModel.LoadAsync().ConfigureAwait(false);
        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> AddViewAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>().ConfigureAwait(false);
        viewPresenter.Add(bundle);
        return bundle;
    }

    public async Task<IXafBundle<TViewModel>> AddViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.CreateBundleAsync(viewModel).ConfigureAwait(false);
        viewPresenter.Add(bundle);
        return bundle;
    }

    public Task AddViewAsync(IXafBundle bundle, object key)
    {
        var viewPresenter = _viewPresenters[key];
        viewPresenter.Add(bundle);
        return Task.CompletedTask;
    }

    public async Task DeactivateViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetBundleAsync(viewModel).ConfigureAwait(false);
        viewPresenter.Deactivate(bundle);
        await bundle.ViewModel.Unload().ConfigureAwait(false);
    }

    public async Task DeactivateViewsAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = await viewPresenter.BundleProvider.GetBundlesAsync<TViewModel>().ConfigureAwait(false);
        foreach (var bundle in bundles.Where(viewPresenter.Deactivate))
        {
            await bundle.ViewModel.Unload().ConfigureAwait(false);
        }
    }

    public async Task DeactivateViewAsync(IXafBundle bundle, object key)
    {
        var viewPresenter = _viewPresenters[key];

        if (viewPresenter.Deactivate(bundle))
        {
            await bundle.ViewModel.Unload().ConfigureAwait(false);
        }
    }

    public async Task RemoveViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetBundleAsync(viewModel).ConfigureAwait(false);
        if (viewPresenter.Remove(bundle))
        {
            await bundle.ViewModel.Unload().ConfigureAwait(false);
        }
    }

    public async Task RemoveViewsAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = await viewPresenter.BundleProvider.GetBundlesAsync<TViewModel>().ConfigureAwait(false);
        foreach (var bundle in bundles.Where(viewPresenter.Remove))
        {
            await bundle.ViewModel.Unload().ConfigureAwait(false);
        }
    }

    public async Task RemoveViewAsync(IXafBundle bundle, object key)
    {
        var viewPresenter = _viewPresenters[key];
        if (viewPresenter.Remove(bundle))
        {
            await bundle.ViewModel.Unload().ConfigureAwait(false);
        }
    }
}
