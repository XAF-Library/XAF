using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;

public interface IViewProvider
{
    IXafViewBundle GetView<TViewModel>()
        where TViewModel : IXafViewModel;
}