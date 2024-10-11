namespace XAF.Core.MVVM;
public interface IXafViewModel : IComparable<IXafViewModel>
{
    Task LoadAsync();

    Task WhenSelected();

    Task WhenUnselected();

    Task UnloadAsync();
}

public interface IXafViewModel<in TParameter> : IXafViewModel
{
    void Prepare(TParameter parameter);
}