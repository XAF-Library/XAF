using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace XAF.UI.Abstraction;

public interface IViewDescriptorCollection
{
    ViewDescriptor AddView(Type viewType);
    void AddDecorator<TAttribute, TDecorator>(Func<TAttribute, TDecorator> action)
        where TAttribute : Attribute;

    void AddDecorator<TAttribute, TDecorator>(Func<IEnumerable<TAttribute>, TDecorator> action)
        where TAttribute : Attribute;

    void AddDescriptorInitilizer<TAttrbiute>(Action<TAttrbiute, ViewDescriptor, IServiceCollection> action)
        where TAttrbiute : Attribute;

    IViewDescriptorProvider BuildViewDescriptorProvider();
}