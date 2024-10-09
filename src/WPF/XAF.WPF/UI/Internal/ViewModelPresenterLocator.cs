using XAF.Core.ExtensionMethods;

namespace XAF.WPF.UI.Internal;
internal class ViewModelPresenterLocator
{
    private readonly Func<object, IViewModelPresenter> _presenterFactory;

    private readonly Dictionary<object, IViewModelPresenter> _presentersByKey;

    public ViewModelPresenterLocator(Func<object, IViewModelPresenter> presenterFactory)
    {
        _presenterFactory = presenterFactory;
        _presentersByKey = [];
    }

    public IViewModelPresenter GetPresenter(object key)
    {
        return _presentersByKey.GetOrAdd(key, _presenterFactory);
    }
}
