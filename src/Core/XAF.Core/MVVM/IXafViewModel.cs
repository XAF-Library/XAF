namespace XAF.Core.MVVM;
public interface IXafViewModel : IComparable<IXafViewModel>
{
    void Prepare();

    Task WhenLoaded();

    Task WhenActivated();

    Task WhenDeactivated();

    Task WhenUnloadAsync();

    Task WaitForViewClose();
}

public interface IXafViewModel<in TParameter> : IXafViewModel
{
    void Prepare(TParameter parameter);
}