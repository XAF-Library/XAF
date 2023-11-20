using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.Abstraction.ExtensionMethods;
public static class XafBundleExtensions
{
    public static IObservable<IChangeSet<IXafViewBundle>> PrepareForViewChange(this IObservable<IChangeSet<IXafViewBundle>> observable, IViewPresenter viewPresenter)
    {
        return observable.Sort(viewPresenter.Comparer)
            .ObserveOn(Schedulers.MainScheduler);
    }
}
