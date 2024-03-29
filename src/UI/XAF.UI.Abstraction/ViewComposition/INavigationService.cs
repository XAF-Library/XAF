﻿using System;
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

    Task NavigateAsync<TViewModel>(object viewPresenterKey)
        where TViewModel : IXafViewModel;

    Task NavigateAsync<TViewModel, TParameter>(TParameter parameter, object viewPresenterKey)
        where TViewModel : IXafViewModel<TParameter>;

    Task NavigateAsync<TViewModel>(TViewModel viewModel, object viewPresenterKey)
        where TViewModel : IXafViewModel;

    Task NavigateAsync<TViewModel, TParameter>(TViewModel viewModel, TParameter parameter, object viewPresenterKey)
        where TViewModel : IXafViewModel<TParameter>;

    Task NavigateAsync(IXafBundle bundle, object viewPresenterKey);

    Task NavigateAsync(IXafBundle bundle, object parameter, object viewPresenterKey);

    Task NavigateAsync(Type viewModelType, object viewPresenterKey);
    
    Task NavigateAsync(Type viewModelType, object parameter, object viewPresenterKey);

    IObservable<bool> CanNavigateBack(object viewPresenterKey);

    IObservable<bool> CanNavigateForward(object viewPresenterKey);

    IObservable<IXafBundle> WhenNavigated(object viewPresenterKey);
}
