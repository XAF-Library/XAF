namespace XAF.WPF.UI;

public record ViewProviderOptions(uint CacheSize)
{
    public static ViewProviderOptions Default { get; } = new(0);
}