namespace XAF.UI.Abstraction.Dialog;
public interface IDialogService
{
    void ShowDialog<T>()
        where T : IActivatableViewModel;

    void ShowDialog<TViewModel, TParameter>(TParameter parameter)
        where TViewModel : IActivatableViewModel<TParameter>;

    TResult? ShowInputDialog<TViewModel, TResult>()
        where TViewModel : IResultViewModel<TResult>;

    TResult? ShowInputDialog<TViewModel, TParameter,TResult>(TParameter parameter)
        where TViewModel: IResultViewModel<TResult, TParameter>;

    void CloseCurrentDialog();
}
