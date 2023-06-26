using System.Linq.Expressions;
using XAF.Utilities.ExtensionMethods;

namespace XAF.Utilities;
public static class NotifyPropertyChangedExtensions
{
    public static void AddValueChangedCallBack<TModel, TProperty>(this TModel viewModel,
       Expression<Func<TModel, TProperty>> propertySelector,
       Action<TProperty> callBack)
       where TModel : NotfiyPropertyChanged
    {
        var name = propertySelector.GetPropertyName();
        viewModel.AddCallBack(callBack, name);
    }
}
