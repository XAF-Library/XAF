using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Dialog;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.Attributes;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Internal;
internal class DialogService : IDialogService
{
    private readonly IViewDescriptorProvider _viewCollection;
    private readonly IViewProvider _viewProvider;
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IViewProvider viewProvider, IServiceProvider serviceProvider)
    {
        _viewCollection = viewProvider.ViewDescriptorProvider;
        _viewProvider = viewProvider;
        _serviceProvider = serviceProvider;
    }

    public void ShowDialog<T>() where T : IDialogViewModel
    {
        var (window, dialogVm) = CreateDialogWindow<T>();

        dialogVm.OnDialogOpened();
        window.ShowDialog();
        dialogVm.OnDialogClosed();
    }

    public void ShowDialog<TViewmodel, TParameter>(TParameter parameter) 
        where TViewmodel : IDialogViewModel<TParameter>
    {
        var (window, dialogVm) = CreateDialogWindow<TViewmodel>();

        dialogVm.OnDialogOpened();
        dialogVm.OnDialogOpend(parameter);
        window.ShowDialog();
        dialogVm.OnDialogClosed();
    }

    public TResult? ShowInputDialog<TViewModel, TResult>() 
        where TViewModel : IInputDialogViewModel<TResult>
    {
        var (window, dialogVm) = CreateDialogWindow<TViewModel>();

        dialogVm.OnDialogOpened();
        window.ShowDialog();
        return dialogVm.OnDialogClosed();
    }

    public TResult? ShowInputDialog<TViewModel, TParameter, TResult>(TParameter parameter) 
        where TViewModel : IInputDialogViewModel<TParameter, TResult>
    {
        var (window, dialogVm) = CreateDialogWindow<TViewModel>();

        dialogVm.OnDialogOpened(parameter);
        window.ShowDialog();
        return dialogVm.OnDialogClosed();
    }

    private (Window window, T viewmodel) CreateDialogWindow<T>()
        where T : IDialogViewModel
    {
        var descriptor = _viewCollection.GetDescriptorForViewModel(typeof(T));
        if (!descriptor.TryGetDecoratorValue<DialogWindowAttribute, Type>(out var windowType))
        {
            windowType = typeof(DialogWindow);
        }

        var window = (Window)_serviceProvider.GetRequiredService(windowType);

        var (view, viewModel) = _viewProvider.GetViewWithViewModel(descriptor);
        window.Content = view;
        var dialogVm = (T)viewModel;
        window.Title = dialogVm.Title;

        return (window, dialogVm);
    }
}
