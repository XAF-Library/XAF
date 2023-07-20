namespace XAF.UI.Abstraction;
public interface INavigationService
{
    uint BackStackCapacity { get; set; }

    void NavigateTo<TViewModel>(object containerKey)
        where TViewModel : INavigationTarget;

    void NavigateTo<TViewModel>(object containerKey, TViewModel viewModel)
        where TViewModel : INavigationTarget;

    void NavigateTo<TViewModel, TParameter>(object containerKey, TParameter parameter)
        where TViewModel : INavigationTarget<TParameter>;

    void NavigateTo(Type viewModelType, object containerKey);

    void AddNavigationCallback(object containerKey, Action<ViewDescriptor, IViewModel> callback);

    bool CanNavigateBack(object containerKey);

    void RegisterCanNavigateBackChangedCallback(object containerKey, Action<bool> callback);

    bool CanNavigateForward(object containerKey);

    void ReigsterCanNavigateForwardChangedCallback(object containerKey, Action<bool> callback);

    void NavigateBack(object containerKey);

    void NavigateForward(object containerKey);
}