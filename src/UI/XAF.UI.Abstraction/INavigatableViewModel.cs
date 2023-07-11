using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction;
public interface INavigationTarget : IViewModel
{
    void OnNavigatedTo();
    void OnNavigatedFrom();
}

public interface INavigationTarget<T> : INavigationTarget
{
    void OnNavigatedTo(T parameter);
}
