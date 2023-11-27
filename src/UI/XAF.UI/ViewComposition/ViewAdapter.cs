using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;

namespace XAF.UI.ViewComposition;
public abstract class ViewAdapter<TView, TViewPresenter> : IViewAdapter
    where TView : class
    where TViewPresenter : IViewPresenter
{
    public Type ViewType { get; } = typeof(TView);

    public Type PresenterType { get; } = typeof(TViewPresenter);

    public void Adapt(object view, IViewPresenter viewPresenter, CompositeDisposable disposables)
    {
        if (view is not TView tView)
        {
            throw new ArgumentException("view is not compatible with adapter");
        }

        if(viewPresenter is not TViewPresenter presenter)
        {
            throw new ArgumentException("presenter is not compatible with adapter");
        }

        Adapt(tView, presenter, disposables);
    }

    public abstract void Adapt(TView view, TViewPresenter viewPresenter, CompositeDisposable disposables);

}
