using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Data;
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
    private readonly Stack<Window> _dialogWindows = new();

    public DialogService(IViewProvider viewProvider, IServiceProvider serviceProvider)
    {
        _viewCollection = viewProvider.ViewDescriptorProvider;
        _viewProvider = viewProvider;
        _serviceProvider = serviceProvider;
    }

    public void CloseCurrentDialog()
    {
        var window = _dialogWindows.Pop();
        window.Close();
    }

    public void ShowDialog<T>() where T
        : IActivatableViewModel
    {
        var (window, dialogVm) = CreateDialogWindow<T>();

        dialogVm.OnActivated();
        window.ShowDialog();
        dialogVm.OnDeactivated();
    }

    public void ShowDialog<TViewModel, TParameter>(TParameter parameter)
        where TViewModel : IActivatableViewModel<TParameter>
    {
        var (window, dialogVm) = CreateDialogWindow<TViewModel>();

        dialogVm.OnActivated(parameter);
        window.ShowDialog();
        dialogVm.OnDeactivated();
    }

    public TResult? ShowInputDialog<TViewModel, TResult>()
        where TViewModel : IResultViewModel<TResult>
    {
        var (window, dialogVm) = CreateDialogWindow<TViewModel>();

        dialogVm.OnActivated();
        window.ShowDialog();
        dialogVm.OnDeactivated();
        return dialogVm.CreateResult();
    }

    public TResult? ShowInputDialog<TViewModel, TParameter, TResult>(TParameter parameter)
        where TViewModel : IResultViewModel<TResult, TParameter>
    {
        var (window, dialogVm) = CreateDialogWindow<TViewModel>();

        dialogVm.OnActivated(parameter);
        window.ShowDialog();
        dialogVm.OnDeactivated();
        return dialogVm.CreateResult();
    }

    private (Window window, T viewmodel) CreateDialogWindow<T>()
        where T : IViewModel
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

        if (dialogVm is IDialogProperties properties)
        {
            var binding = new Binding(nameof(properties.Title))
            {
                Source = properties,
            };

            BindingOperations.SetBinding(window, Window.TitleProperty, binding);

            window.Closed += Window_Closed;
        }
        _dialogWindows.Push(window);

        return (window, dialogVm);
    }

    private void Window_Closed(object? sender, EventArgs e)
    {
        var window = (Window)sender!;
        window.Closed -= Window_Closed;

        var stackWindow = _dialogWindows.Peek();
        
        if (window == stackWindow)
        {
            _dialogWindows.Pop();
        }

        BindingOperations.ClearAllBindings(window);
    }
}
