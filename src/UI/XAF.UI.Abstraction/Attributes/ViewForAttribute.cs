using System.ComponentModel;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewModels;

namespace XAF.UI.Abstraction.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]


[EditorBrowsable(EditorBrowsableState.Never)]
public class ViewForAttribute : BundleDecoratorAttribute
{
    public Type ViewModelType { get; }

    public ViewForAttribute(Type forType)
    {
        ViewModelType = forType;
    }
}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class ViewForAttribute<TViewModel> : ViewForAttribute
    where TViewModel : IXafViewModel
{
    public Type ViewModelType { get; }
    
    public ViewForAttribute()
        : base(typeof(TViewModel))
    {
    }

}