namespace XAF.UI.Abstraction;
public interface INavigationService
{
    uint BackStackCapacity { get; set; }

    void NavigateTo<TViewModel>(object containerKey)
        where TViewModel : IActivatableViewModel;

    void NavigateTo<TViewModel>(object containerKey, TViewModel viewModel)
        where TViewModel : IActivatableViewModel;

    void NavigateTo<TViewModel, TParameter>(object containerKey, TParameter parameter)
        where TViewModel : IActivatableViewModel<TParameter>;

    void NavigateTo(Type viewModelType, object containerKey);    

    void AddNavigationCallback(object containerKey, Action<ViewDescriptor, IViewModel> callback);

    bool CanNavigateBack(object containerKey);

    bool CanNavigateForward(object containerKey);    

    void NavigateBack(object containerKey);

    void NavigateForward(object containerKey);
}