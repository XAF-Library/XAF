using XAF.UI.Abstraction;

namespace XAF.UI.Abstraction.Attributes;

public class ViewForAttribute<TViewModel> : ViewForAttribute
    where TViewModel : IViewModel
{
    public ViewForAttribute()
        : base(typeof(TViewModel))
    {
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class ViewForAttribute : Attribute
{
    public Type ViewModelType { get; }

    public ViewForAttribute(Type viewModelType)
    {
        ViewModelType = viewModelType;
    }
}
