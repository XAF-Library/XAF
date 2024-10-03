namespace XAF.WPF.UI;
public interface IViewModelPresenterFactory
{
    IViewModelPresenter CreateViewModelPresenter(object key);
}
