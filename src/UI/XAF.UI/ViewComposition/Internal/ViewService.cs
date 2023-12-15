using System;
using System.Collections.Generic;
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

    public async Task ActivateViewsAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {

        var viewPresenter = _viewPresenters[key];

        var bundles = await viewPresenter.BundleProvider.GetBundlesAsync<TViewModel>();

        if (bundles.Any())
        {
            foreach (var bundle in bundles)
            {
                bundle.ViewModel.Preload();
                viewPresenter.Activate(bundle);
                await bundle.ViewModel.LoadAsync();
            }

            return;
        }

        var newBundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>();
        newBundle.ViewModel.Preload();
        viewPresenter.Activate(newBundle);
        await newBundle.ViewModel.LoadAsync();
    }

    public async Task ActivateViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {

        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync(viewModel);
        bundle.ViewModel.Preload();
        viewPresenter.Activate(bundle);
        await viewModel.LoadAsync();
    }

    public async Task ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, TViewModel viewModel, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync(viewModel);
        viewModel.Preload();
        viewModel.Preload(parameter);
        viewPresenter.Activate(bundle);
        await viewModel.LoadAsync();
    }

    public async Task ActivateViewAsync<TViewModel, TParameter>(TParameter parameter, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetOrCreateBundleAsync<TViewModel>();
        bundle.ViewModel.Preload();
        bundle.ViewModel.Preload(parameter);
        viewPresenter.Activate(bundle);
        await bundle.ViewModel.LoadAsync();
    }

    public async Task ActivateViewsAsync<TViewModel, TParameter>(TParameter parameter, object key) where TViewModel : IXafViewModel<TParameter>
    {
        var viewPresenter = _viewPresenters[key];

        var bundles = await viewPresenter.BundleProvider.GetBundlesAsync<TViewModel>();

        if (bundles.Any())
        {
            foreach (var bundle in bundles)
            {
                bundle.ViewModel.Preload();
                bundle.ViewModel.Preload(parameter);
                viewPresenter.Activate(bundle);
                await bundle.ViewModel.LoadAsync();
            }

            return;
        }

        var newBundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>();
        newBundle.ViewModel.Preload();
        newBundle.ViewModel.Preload(parameter);
        viewPresenter.Activate(newBundle);
        await newBundle.ViewModel.LoadAsync();
    }

    public async Task AddViewAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.CreateBundleAsync<TViewModel>();
        viewPresenter.Add(bundle);
    }

    public async Task AddViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];

        var bundle = await viewPresenter.BundleProvider.CreateBundleAsync(viewModel);
        viewPresenter.Add(bundle);
    }

    public async Task DeactivateViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetBundleAsync(viewModel);
        viewPresenter.Deactivate(bundle);
        await bundle.ViewModel.Unload();
    }

    public async Task DeactivateViewsAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = await viewPresenter.BundleProvider.GetBundlesAsync<TViewModel>();

        foreach (var bundle in bundles)
        {
            viewPresenter.Deactivate(bundle);
            await bundle.ViewModel.Unload();
        }
    }

    public async Task RemoveViewAsync<TViewModel>(TViewModel viewModel, object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundle = await viewPresenter.BundleProvider.GetBundleAsync(viewModel);
        viewPresenter.Remove(bundle);
        await bundle.ViewModel.Unload();
    }

    public async Task RemoveViewsAsync<TViewModel>(object key) where TViewModel : IXafViewModel
    {
        var viewPresenter = _viewPresenters[key];
        var bundles = await viewPresenter.BundleProvider.GetBundlesAsync<TViewModel>();
        foreach (var bundle in bundles)
        {
            viewPresenter.Remove(bundle);
            await bundle.ViewModel.Unload();
        }
    }
}
