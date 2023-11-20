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
    CompositeDisposable Disposables { get; }

    void Add(IXafViewBundle view);

    void Remove(IXafViewBundle view);

    void Activate(IXafViewBundle view);

    void Deactivate(IXafViewBundle view);

    ISubject<IComparer<IXafViewBundle>> Comparer { get; }

    IObservableList<IXafViewBundle> Views { get; }

    IObservableList<IXafViewBundle> ActiveViews { get; }
}

public interface IViewPresenter<TView> : IViewPresenter
    where TView : class
{
    void Connect(TView targetView);
}
