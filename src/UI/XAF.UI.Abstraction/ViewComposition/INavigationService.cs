using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface INavigationService
{
    Task NavigateBack(object viewPresenterKey);

    Task NavigateForward(object viewPresenterKey);

    Task NavigateTo<TViewModel>(object viewPresenterKey)
        where TViewModel : IXafViewModel;

    Task NavigateTo<TViewModel, TParameter>(TParameter parameter, object viewPresenterKey)
        where TViewModel : IXafViewModel<TParameter>;

    Task NavigateTo<TViewModel>(TViewModel viewModel, object viewPresenterKey)
        where TViewModel : IXafViewModel;

    Task NavigateTo<TViewModel, TParameter>(TViewModel viewModel, TParameter parameter, object viewPresenterKey)
        where TViewModel : IXafViewModel<TParameter>;

    IObservable<bool> CanNavigateBack(object viewPresenterKey);

    IObservable<bool> CanNavigateForward(object viewPresenterKey);

    IObservable<IXafBundle> WhenNavigated(object viewPresenterKey);
}
