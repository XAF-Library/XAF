namespace XAF;
public interface IViewModel
{
    void OnViewStateChanged(ViewState oldState, ViewState newState);
    void OnViewLoaded();
}