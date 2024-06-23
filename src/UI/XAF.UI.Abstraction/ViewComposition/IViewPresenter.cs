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
    Task AddAsync(IXafBundle view);

    Task<bool> RemoveAsync(IXafBundle view);

    Task ActivateAsync(IXafBundle view);

    Task<bool> DeactivateAsync(IXafBundle view);

    void Connect(object view);

    void Disconnect(object view);

    IBundleProvider BundleProvider { get; }

    ISubject<IComparer<IXafBundle>> Comparer { get; }

    IObservableList<IXafBundle> Views { get; }

    IObservableList<IXafBundle> ActiveViews { get; }
}