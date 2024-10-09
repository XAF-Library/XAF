using System.ComponentModel;
using XAF.Core.MVVM;

namespace XAF.WPF.UI.Attributes;

[EditorBrowsable(EditorBrowsableState.Never)]
public class ViewForAttribute : Attribute
{
    public ViewForAttribute(Type viewModelType, PresentationType presentationType)
    {
        ViewModelType = viewModelType;
        PresentationType = presentationType;
    }

    public PresentationType PresentationType { get; }

    public Type ViewModelType { get; }
}

public sealed class ViewForAttribute<TViewModel> : ViewForAttribute
    where TViewModel : class, IXafViewModel
{
    public ViewForAttribute(PresentationType presentationType = PresentationType.Both)
        : base(typeof(TViewModel), presentationType)
    {

    }
}
