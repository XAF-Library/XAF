namespace XAF.UI.Abstraction.ViewComposition;
public interface IViewAdapterCollection
{
    IViewAdapter GetAdapterFor(Type viewType);

    void AddAdapter<TViewAdapter>()
        where TViewAdapter : IViewAdapter;

    void AddAdapter(Type adapterType);
}
