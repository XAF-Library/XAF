namespace XAF.UI.Abstraction.Dialog;
public interface IDialogService
{
    void ShowDialog<T>()
        where T : IDialogViewModel;

    void ShowDialog<TViewmodel, TParameter>(TParameter parameter)
        where TViewmodel : IDialogViewModel<TParameter>;

    TResult? ShowInputDialog<TViewModel, TResult>()
        where TViewModel : IInputDialogViewModel<TResult>;

    TResult? ShowInputDialog<TViewModel, TParameter, TResult>(TParameter parameter)
        where TViewModel: IInputDialogViewModel<TParameter,TResult>;
}
