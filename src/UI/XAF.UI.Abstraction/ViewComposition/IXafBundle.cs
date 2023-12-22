using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.ViewComposition;
public interface IXafBundle : IComparable<IXafBundle>
{
    IXafViewModel ViewModel { get; }

    object View { get; }

    IBundleMetadata Metadata { get; }
}

public interface IXafBundle<TViewModel> : IXafBundle
    where TViewModel : IXafViewModel 
{
    new TViewModel ViewModel { get; }
}