namespace XAF.UI.Abstraction.ViewComposition;
public interface IBundleMetadata
{
    Type ViewModelType { get; }

    Type ViewType { get; }

    Type? ParameterType { get; }

    IBundleDecoratorCollection ViewDecorators { get; }
}
