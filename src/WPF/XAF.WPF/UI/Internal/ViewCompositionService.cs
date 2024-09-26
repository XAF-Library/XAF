using XAF.Core;
using XAF.Core.MVVM;
using XAF.Core.UI;

namespace XAF.WPF.UI.Internal;
internal sealed class ViewCompositionService : IViewCompositionService
{
    public event AsyncEventHandler<ViewManipulationEventArgs> ViewManipulationRequested;
    public event AsyncEventHandler<ViewManipulationEventArgs> ViewManipulationCompleted;

    public async Task<bool> AddViewAsync<TViewModel>(TViewModel vm, object presenterKey) where TViewModel : IXafViewModel
    {
        var canceled = await OnViewManipulationRequested(ManipulationType.AddView, vm, presenterKey).ConfigureAwait(false);

        if (canceled)
        {
            return false;
        }
    }

    public Task<bool> AddViewAsync<TViewModel, TParameter>(TViewModel vm, TParameter parameter, object presenterKey) where TViewModel : IXafViewModel<TParameter>
    {
        throw new NotImplementedException();
    }

    public Task<bool> CloseWindowAsync<TViewModel>(TViewModel vm) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveViewAsync<TViewModel>(TViewModel vm, object presenterKey) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task<bool> OpenWindowAsync<TViewModel>() where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task<bool> OpenWindowAsync<TViewModel>(TViewModel vm) where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }

    public Task<bool> OpenWindowAsync<TViewModel, TParameter>(TViewModel vm, TParameter parameter) where TViewModel : IXafViewModel<TParameter>
    {
        throw new NotImplementedException();
    }

    Task<bool> IViewCompositionService.AddViewAsync<TViewModel>(object presenterKey)
    {
        throw new NotImplementedException();
    }

    Task<bool> IViewCompositionService.AddViewAsync<TViewModel, TParameter>(object presenterKey, TParameter parameter)
    {
        throw new NotImplementedException();
    }

    Task<bool> IViewCompositionService.OpenWindowAsync<TViewModel, TParameter>(TParameter parameter)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="vm"></param>
    /// <param name="presenterKey"></param>
    /// <param name="parameter"></param>
    /// <returns>true if an delegate cancels the manipulation, otherwise false</returns>
    protected async Task<bool> OnViewManipulationRequested(ManipulationType type, IXafViewModel vm, object presenterKey, object? parameter = null)
    {
        var args = new ViewManipulationEventArgs(type, vm, presenterKey, parameter);

        if (ViewManipulationRequested is null)
        {
            return false;
        }

        await ViewManipulationRequested.InvokeAsync(this, args).ConfigureAwait(false);

        return args.Cancle;
    }
}
