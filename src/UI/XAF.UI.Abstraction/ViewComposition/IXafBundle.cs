using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IXafBundle : IComparable<IXafBundle>, IBundleMetadata
{
    IXafViewModel ViewModel { get; }

    object View { get; }
}

public interface IXafBundle<TViewModel> : IXafBundle
    where TViewModel : IXafViewModel 
{
    new TViewModel ViewModel { get; }
}