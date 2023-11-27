namespace XAF.UI.Abstraction.ViewComposition;
public interface IBundleMetadata
{
    Type ViewModelType { get; }

    Type ViewType { get; }

    IBundleDecoratorCollection ViewDecorators { get; }
}
