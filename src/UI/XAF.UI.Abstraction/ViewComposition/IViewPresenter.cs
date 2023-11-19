using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IViewPresenter : IDisposable
{
    object Key { get; }

    CompositeDisposable Disposables { get; }

    object Target { get; }

    void Add(IXafView view);

    void Remove(IXafView view);

    void Activate(IXafView view);

    void Deactivate(IXafView view);

    ISubject<IComparer<IXafView>> Comparer { get; }

    IObservableList<IXafView> Views { get; }

    IObservableList<IXafView> ActiveViews { get; }

    void Connect(object targetView);
}
