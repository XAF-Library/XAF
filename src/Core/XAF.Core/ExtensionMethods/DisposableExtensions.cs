using System.Reactive.Disposables;

namespace XAF.Core.ExtensionMethods;
public static class DisposableExtensions
{
    public static void DisposeWith(this IDisposable disposable, CompositeDisposable disposables)
    {
        disposables.Add(disposable);
    }
}
