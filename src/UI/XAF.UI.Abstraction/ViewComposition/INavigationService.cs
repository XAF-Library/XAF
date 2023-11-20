using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction.ViewComposition;
public interface INavigationService
{
    Task NavigateBack(object key);

    Task NavigateForward(object key);

    IObservable<bool> CanNavigateBack { get; }

    IObservable<bool> CanNavigateForward { get; }

    IObservable<IXafViewBundle> WhenNavigated(object key);
}
