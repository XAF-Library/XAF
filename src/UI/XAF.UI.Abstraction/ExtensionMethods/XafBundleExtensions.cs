using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.Abstraction.ExtensionMethods;
public static class XafBundleExtensions
{
    public static IObservable<IChangeSet<IXafBundle>> PrepareForViewChange(this IObservable<IChangeSet<IXafBundle>> observable, IViewPresenter viewPresenter)
    {
        return observable.Sort(viewPresenter.Comparer)
            .ObserveOn(Schedulers.MainScheduler);
    }
}
