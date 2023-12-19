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
    void Add(IXafBundle view);

    bool Remove(IXafBundle view);

    void Activate(IXafBundle view);

    bool Deactivate(IXafBundle view);

    void Connect(object view);

    void Disconnect(object view);

    IBundleProvider BundleProvider { get; }

    ISubject<IComparer<IXafBundle>> Comparer { get; }

    IObservableList<IXafBundle> Views { get; }

    IObservableList<IXafBundle> ActiveViews { get; }
}