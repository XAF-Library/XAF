using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Utilities.ExtensionMethods;
public static class DisposableExtensions
{
    public static void DiposeWith(this IDisposable disposable, CompositeDisposable disposables)
    {
        disposables.Add(disposable);
    }
}
