using System.Reactive.Linq;
using System.Reactive.Subjects;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;
using XAF.Utilities.ExtensionMethods;

namespace XAF.UI.ViewComposition.Internal;
internal class NavigationService : INavigationService
{
    private readonly IViewService _viewService;
    private readonly Dictionary<object, NavigationStack> _navigationStacks = new();
    private readonly Dictionary<object, ISubject<IXafBundle>> _navigationObservables = new();

    public NavigationService(IViewService viewService)
    {
        _viewService = viewService;
    }

    public IObservable<bool> CanNavigateBack(object viewPresenterKey)
    {
        if (_navigationStacks.TryGetValue(viewPresenterKey, out var navigationStack))
        {
            return navigationStack.CanNavigateBack()
                .DistinctUntilChanged();
        }

        if (_viewService.ContainsPresenter(viewPresenterKey))
        {
            var stack = new NavigationStack();
            _navigationStacks[viewPresenterKey] = stack;
            return stack.CanNavigateBack()
                .DistinctUntilChanged();
        }

        return Observable.Return(false);
    }

    public IObservable<bool> CanNavigateForward(object viewPresenterKey)
    {
        if (_navigationStacks.TryGetValue(viewPresenterKey, out var navigationStack))
        {
            return navigationStack.CanNavigateForward()
                .DistinctUntilChanged();
        }

        if (_viewService.ContainsPresenter(viewPresenterKey))
        {
            var stack = new NavigationStack();
            _navigationStacks[viewPresenterKey] = stack;
            return stack.CanNavigateForward()
                .DistinctUntilChanged();
        }

        return Observable.Return(false);
    }

    public Task NavigateBack(object viewPresenterKey)
    {
        if (!_navigationStacks.TryGetValue(viewPresenterKey, out var stack))
        {
            return Task.CompletedTask;
        }

        var entry = stack.NavigateBack();

        if (entry.Parameter is not null && entry.Bundle.Metadata.ParameterType is not null)
        {
            return NavigateToInternal(entry.Bundle, entry.Parameter, viewPresenterKey, false);
        }

        return NavigateToInternal(entry.Bundle, viewPresenterKey, false);
    }

    public Task NavigateForward(object viewPresenterKey)
    {
        if (!_navigationStacks.TryGetValue(viewPresenterKey, out var stack))
        {
            return Task.CompletedTask;
        }

        var entry = stack.NavigateForward();

        if (entry.Parameter is not null && entry.Bundle.Metadata.ParameterType is not null)
        {
            return NavigateToInternal(entry.Bundle, entry.Parameter, viewPresenterKey, false);
        }

        return NavigateToInternal(entry.Bundle, viewPresenterKey, false);

    }

    public async Task NavigateTo<TViewModel>(object viewPresenterKey) where TViewModel : IXafViewModel
    {
        var bundle = await _viewService.ActivateFirstViewAsync<TViewModel>(viewPresenterKey).ConfigureAwait(false);
        RecordNavigation(bundle, viewPresenterKey);
    }

    public async Task NavigateTo<TViewModel, TParameter>(TParameter parameter, object viewPresenterKey)
        where TViewModel : IXafViewModel<TParameter>
    {
        var bundle = await _viewService.ActivateViewAsync<TViewModel, TParameter>(parameter, viewPresenterKey).ConfigureAwait(false);
        RecordNavigation(bundle, parameter, viewPresenterKey);
    }

    public async Task NavigateTo<TViewModel>(TViewModel viewModel, object viewPresenterKey) where TViewModel : IXafViewModel
    {
        var bundle = await _viewService.ActivateViewAsync(viewModel, viewPresenterKey).ConfigureAwait(false);
        RecordNavigation(bundle, viewPresenterKey);
    }

    public async Task NavigateTo<TViewModel, TParameter>(TViewModel viewModel, TParameter parameter, object viewPresenterKey)
        where TViewModel : IXafViewModel<TParameter>
    {
        var bundle = await _viewService.ActivateViewAsync(parameter, viewModel, viewPresenterKey);
        RecordNavigation(bundle, parameter, viewPresenterKey);
    }

    public Task NavigateTo(IXafBundle bundle, object viewPresenterKey)
    {
        return NavigateToInternal(bundle, viewPresenterKey, true);
    }

    public Task NavigateTo(IXafBundle bundle, object parameter, object viewPresenterKey)
    {
        return NavigateToInternal(bundle, parameter, viewPresenterKey, true);
    }

    public IObservable<IXafBundle> WhenNavigated(object viewPresenterKey)
    {
        if (!_navigationObservables.TryGetValue(viewPresenterKey, out var observable))
        {
            observable = new Subject<IXafBundle>();
            _navigationObservables.Add(viewPresenterKey, observable);
        }

        return observable;
    }

    private async Task NavigateToInternal(IXafBundle bundle, object viewPresenterKey, bool record)
    {
        await _viewService.ActivateViewAsync(bundle, viewPresenterKey);
        if (record)
        {
            RecordNavigation(bundle, viewPresenterKey);
        }
    }

    private async Task NavigateToInternal(IXafBundle bundle, object parameter, object viewPresenterKey, bool record)
    {
        await _viewService.ActivateViewAsync(bundle, parameter, viewPresenterKey);
        if (record)
        {
            RecordNavigation(bundle, parameter, viewPresenterKey);
        }
    }

    private void RecordNavigation(IXafBundle bundle, object key)
    {
        RecordNavigation(bundle, null, key);
    }

    private void RecordNavigation(IXafBundle bundle, object? parameter, object key)
    {
        var stack = _navigationStacks.GetOrAdd(key);
        if(stack.CurrentEntry is not null && bundle == stack.CurrentEntry.Bundle && parameter  == stack.CurrentEntry.Parameter)
        {
            return;
        }

        stack.RecordNavigation(bundle, parameter);

        if (_navigationObservables.TryGetValue(key, out var subject))
        {
            subject.OnNext(bundle);
        }
    }
}
