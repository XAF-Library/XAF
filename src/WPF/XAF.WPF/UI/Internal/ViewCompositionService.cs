using Microsoft.Extensions.DependencyInjection;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using XAF.Core.MVVM;
using XAF.Core.UI;

namespace XAF.WPF.UI.Internal;
internal sealed class ViewCompositionService : IViewCompositionService
{
    private readonly ViewModelPresenterLocator _presenterRepository;
    private readonly IServiceProvider _serviceProvider;
    private readonly Subject<ViewManipulation> _viewManipulationRequestedSubject;
    private readonly Subject<ViewManipulation> _viewManipulationCompletedSubject;

    public ViewCompositionService(ViewModelPresenterLocator presenterRepository, IServiceProvider serviceProvider)
    {
        _viewManipulationCompletedSubject = new();
        _viewManipulationRequestedSubject = new();
        _presenterRepository = presenterRepository;
        _serviceProvider = serviceProvider;
    }

    public Task<bool> AddViewAsync<TViewModel>(object presenterKey, CancellationToken cancellation) where TViewModel : class, IXafViewModel
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        var manipulation = new ViewManipulation(tokenSource, ViewManipulationType.Add, vm, presenterKey);

        _viewManipulationRequestedSubject.OnNext(manipulation);
        var presenter = _presenterRepository.GetPresenter(presenterKey);

        if (tokenSource.IsCancellationRequested)
        {
            return Task.FromResult(false);
        }

        if (!presenter.Add(vm, tokenSource.Token))
        {
            return Task.FromResult(false);
        }

        _viewManipulationCompletedSubject.OnNext(manipulation);
        return Task.FromResult(true);
    }

    public async Task<bool> AddViewAsync<TViewModel, TParameter>(TParameter parameter, object presenterKey, CancellationToken cancellation) where TViewModel : class, IXafViewModel<TParameter>
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

        var vm = _serviceProvider.GetRequiredService<TViewModel>();
        var manipulation = new ViewManipulation(tokenSource, ViewManipulationType.Add, vm, presenterKey, parameter);

        _viewManipulationRequestedSubject.OnNext(manipulation);
        var presenter = _presenterRepository.GetPresenter(presenterKey);

        if (tokenSource.IsCancellationRequested)
        {
            return false;
        }

        await vm.PrepareAsync(parameter);

        if (!presenter.Add(vm, tokenSource.Token))
        {
            return false;
        }

        _viewManipulationCompletedSubject.OnNext(manipulation);
        return true;
    }

    public Task<bool> AddViewAsync<TViewModel>(TViewModel vm, object presenterKey, CancellationToken cancle) where TViewModel : IXafViewModel
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancle);

        var manipulation = new ViewManipulation(tokenSource, ViewManipulationType.Add, vm, presenterKey);

        _viewManipulationRequestedSubject.OnNext(manipulation);
        var presenter = _presenterRepository.GetPresenter(presenterKey);

        if (tokenSource.IsCancellationRequested)
        {
            return Task.FromResult(false);
        }

        if (!presenter.Add(vm, tokenSource.Token))
        {
            return Task.FromResult(false);
        }

        _viewManipulationCompletedSubject.OnNext(manipulation);
        return Task.FromResult(true);
    }

    public async Task<bool> AddViewAsync<TViewModel, TParameter>(TViewModel vm, TParameter parameter, object presenterKey, CancellationToken cancle) where TViewModel : IXafViewModel<TParameter>
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancle);

        var manipulation = new ViewManipulation(tokenSource, ViewManipulationType.Add, vm, presenterKey, parameter);

        _viewManipulationRequestedSubject.OnNext(manipulation);
        var presenter = _presenterRepository.GetPresenter(presenterKey);

        if (tokenSource.IsCancellationRequested)
        {
            return false;
        }

        await vm.PrepareAsync(parameter).ConfigureAwait(false);

        if (!presenter.Add(vm, tokenSource.Token))
        {
            return false;
        }

        _viewManipulationCompletedSubject.OnNext(manipulation);
        return true;
    }

    public Task<bool> RemoveViewAsync<TViewModel>(TViewModel vm, object presenterKey, CancellationToken cancle) where TViewModel : IXafViewModel
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancle);

        var manipulation = new ViewManipulation(tokenSource, ViewManipulationType.Remove, vm, presenterKey);

        _viewManipulationRequestedSubject.OnNext(manipulation);
        var presenter = _presenterRepository.GetPresenter(presenterKey);

        if (tokenSource.IsCancellationRequested)
        {
            return Task.FromResult(false);
        }

        if (!presenter.Remove(vm, tokenSource.Token))
        {
            return Task.FromResult(false);
        }

        _viewManipulationCompletedSubject.OnNext(manipulation);
        return Task.FromResult(true);
    }

    public Task<bool> SelectViewAsync<TViewModel>(TViewModel vm, object presenterKey, CancellationToken cancellation) where TViewModel : IXafViewModel
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

        var manipulation = new ViewManipulation(tokenSource, ViewManipulationType.Select, vm, presenterKey);

        _viewManipulationRequestedSubject.OnNext(manipulation);
        var presenter = _presenterRepository.GetPresenter(presenterKey);

        if (tokenSource.IsCancellationRequested)
        {
            return Task.FromResult(false);
        }

        if (!presenter.Select(vm, tokenSource.Token))
        {
            return Task.FromResult(false);
        }

        _viewManipulationCompletedSubject.OnNext(manipulation);
        return Task.FromResult(true);
    }

    public async Task<bool> SelectViewAsync<TViewModel, TParameter>(TViewModel vm, TParameter parameter, object presenterKey, CancellationToken cancellation) where TViewModel : IXafViewModel<TParameter>
    {
        var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellation);

        var manipulation = new ViewManipulation(tokenSource, ViewManipulationType.Select, vm, presenterKey, parameter);

        _viewManipulationRequestedSubject.OnNext(manipulation);
        var presenter = _presenterRepository.GetPresenter(presenterKey);

        if (tokenSource.IsCancellationRequested)
        {
            return false;
        }

        await vm.PrepareAsync(parameter).ConfigureAwait(false);

        if (!presenter.Select(vm, tokenSource.Token))
        {
            return false;
        }

        _viewManipulationCompletedSubject.OnNext(manipulation);
        return true;
    }

    public IObservable<ViewManipulation> ViewManipulationCompleted()
    {
        return _viewManipulationCompletedSubject;
    }

    public IObservable<ViewManipulation> ViewManipulationCompleted(object presenterKey)
    {
        return _viewManipulationCompletedSubject.Where(m => m.PresenterKey == presenterKey);
    }

    public IObservable<ViewManipulation> ViewManipulationRequested()
    {
        return _viewManipulationRequestedSubject;
    }

    public IObservable<ViewManipulation> ViewManipulationRequested(object presenterKey)
    {
        return _viewManipulationRequestedSubject.Where(m => m.PresenterKey == presenterKey);
    }
}
