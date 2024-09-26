using XAF.Core.MVVM;

namespace XAF.WPF.UI.Attributes;
public sealed class ViewForAttribute<TViewModel>
    where TViewModel : class, IXafViewModel
{
    public ViewForAttribute(PresentationType presentationType)
    {
        PresentationType = presentationType;
    }

    public PresentationType PresentationType { get; }
}
