using System.Diagnostics.CodeAnalysis;
using XAF.UI.Abstraction.Attributes;

namespace XAF.UI.Abstraction.ViewComposition;

public interface IBundleDecoratorCollection
{
    bool Contains<TViewDecorator>()
        where TViewDecorator : BundleDecoratorAttribute;

    TViewDecorator GetDecoratorFirst<TViewDecorator>()
        where TViewDecorator : BundleDecoratorAttribute;

    bool TryGetDecorator<TViewDecorator>([NotNullWhen(true)] out TViewDecorator? decorator)
        where TViewDecorator : BundleDecoratorAttribute;

    IEnumerable<TViewDecorator> GetDecorators<TViewDecorator>()
        where TViewDecorator : BundleDecoratorAttribute;

    IEnumerable<BundleDecoratorAttribute> GetAllDecorators();

    IEnumerable<Type> GetAllDecoratorTypes();
}