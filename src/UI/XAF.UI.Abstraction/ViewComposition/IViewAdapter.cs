using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IViewAdapter
{
    Type ViewType { get; }
    
    Type PresenterType { get; }

    void Adapt(object view, IViewPresenter viewPresenter, CompositeDisposable disposables);

}
