using System.Reactive.Concurrency;
using XAF.UI.Abstraction.ReactiveProperty;

namespace XAF.UI.ReactiveProperty;
public static class RxPropertyExtensions
{
    public static RxProperty<TProperty> ToRxProperty<TProperty>(this TProperty property,
            IScheduler? eventScheduler = null,
            RxPropertySettings settings = RxPropertySettings.Default,
            IEqualityComparer<TProperty>? equalityComparer = null)
    {
        return new RxProperty<TProperty>(property, eventScheduler, settings, equalityComparer);
    }
}
