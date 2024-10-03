using XAF.Core.ExtensionMethods;

namespace XAF.WPF.UI.Internal;
internal class ViewModelPresenterRepository
{
    private readonly IViewModelPresenterFactory _presenterFactory;

    private readonly Dictionary<object, IViewModelPresenter> _presentersByKey;

    public ViewModelPresenterRepository(IViewModelPresenterFactory presenterFactory)
    {
        _presenterFactory = presenterFactory;
    }

    public IViewModelPresenter GetPresenter(object key)
    {
        return _presentersByKey.GetOrAdd(key, _presenterFactory.CreateViewModelPresenter);
    }
}
