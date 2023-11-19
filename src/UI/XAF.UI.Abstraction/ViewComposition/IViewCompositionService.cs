using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IViewCompositionService
{
    IViewPresenter GetPresenter(object key);

    void Add<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    void Add<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    void Remove<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    void Remove<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    void Activate<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    void Activate<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

    void Deactivate<TViewModel>(object key)
        where TViewModel : IXafViewModel;

    void Deactivate<TViewModel>(TViewModel viewModel, object key)
        where TViewModel : IXafViewModel;

}
