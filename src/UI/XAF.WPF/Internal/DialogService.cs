using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Dialog;
using XAF.UI.WPF.ViewComposition;

namespace XAF.UI.WPF.Internal;
internal class DialogService : IDialogService
{
    private readonly IReadOnlyViewCollection _viewCollection;
    private readonly IViewProvider _viewProvider;
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IViewProvider viewProvider, IServiceProvider serviceProvider)
    {
        _viewCollection = viewProvider.ViewCollection;
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
        where T : IViewModel
    {
        var descriptor = _viewCollection.GetDescriptorForViewModel(typeof(T));
        Type? dialogWindowType = null;
        if (descriptor.Properties.TryGetValue(ViewDescriptorKeys.HasSpecialDialogWindowKey, out var windowType))
        {
            dialogWindowType = (Type)windowType;
        }

        var window = dialogWindowType != null
            ? (Window)_serviceProvider.GetRequiredService(dialogWindowType)
            : new DialogWindow();

        var (view, viewModel) = _viewProvider.GetViewWithViewModel(descriptor);

        window.Content = view;
        var dialogVm = (T)viewModel;

        return (window, dialogVm);
    }
}
