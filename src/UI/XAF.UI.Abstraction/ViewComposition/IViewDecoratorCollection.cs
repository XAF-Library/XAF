using System.Diagnostics.CodeAnalysis;

namespace XAF.UI.Abstraction.ViewComposition;

public interface IViewDecoratorCollection
{
    bool Contains<TViewDecorator>()
        where TViewDecorator : IViewDecorator;

    TViewDecorator GetDecorator<TViewDecorator>()
        where TViewDecorator : IViewDecorator;

    bool TryGetViewDecorator<TViewDecorator>([NotNullWhen(true)]out TViewDecorator? decorator);

    IEnumerable<TViewDecorator> GetViewDecorators<TViewDecorator>()
        where TViewDecorator : IViewDecorator;
}