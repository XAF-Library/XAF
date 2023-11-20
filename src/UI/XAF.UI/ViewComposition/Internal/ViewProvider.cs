using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.ViewComposition.Internal;
internal class ViewProvider : IViewProvider
{


    public void Add(Type viewType, Type viewModelType)
    {
        if (!viewType.IsAssignableTo(typeof(IXafViewBundle)))
        {
            throw new ArgumentException($"view must implement the {typeof(IXafViewBundle)} interface", nameof(viewType));
        }
    }

    public void Add<TViewModel, TView>()
        where TViewModel : IXafViewModel
        where TView : IXafViewBundle
    {
        throw new NotImplementedException();
    }

    public IXafViewBundle GetView<TViewModel>() where TViewModel : IXafViewModel
    {
        throw new NotImplementedException();
    }
}
