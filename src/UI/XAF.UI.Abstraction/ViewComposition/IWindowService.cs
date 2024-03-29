﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IWindowService
{
    Task ShowAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task ShowAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    Task ShowAsync(IXafBundle bundle);

    Task<TViewModel> ShowDialogAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task ShowDialogAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;

    Task<TViewModel> ShowDialogAsync<TViewModel, TParameter>(TParameter parameter)
        where TViewModel : IXafViewModel<TParameter>;

    Task ShowDialogAsync<TViewModel, TParameter>(TViewModel viewModel, TParameter parameter)
    where TViewModel : IXafViewModel<TParameter>;

    Task ShowDialogAsync(IXafBundle bundle);

    Task ShowDialogAsync<TParameter>(IXafBundle bundle, TParameter parameter);

    Task CloseAsync<TViewModel>()
        where TViewModel : IXafViewModel;

    Task CloseAsync<TViewModel>(TViewModel viewModel)
        where TViewModel : IXafViewModel;
    
    Task PrepareShells();

    Task ShowShells();

    Task LoadShells();


    void SetDefaultWindowType<TWindow>()
        where TWindow : class;

    void SetDefaultWindowType(Type type);
}
