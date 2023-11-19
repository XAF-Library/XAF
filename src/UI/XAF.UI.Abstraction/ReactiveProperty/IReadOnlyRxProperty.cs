using System.ComponentModel;

namespace XAF.UI.Abstraction.ReactiveProperty;

public interface IReadOnlyRxProperty : INotifyPropertyChanged
{
    object? Value { get; }
}

public interface IReadOnlyRxProperty<out T> : IReadOnlyRxProperty, IObservable<T>, IDisposable
{
    new T Value { get; }

    bool IsDisposed { get; }

    RxPropertySettings Settings { get; }
}
