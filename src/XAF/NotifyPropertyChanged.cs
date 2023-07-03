using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XAF.Utilities;
public abstract class NotifyPropertyChanged : INotifyPropertyChanged
{
    protected readonly Dictionary<string, List<Action<object?>>> PropertyChangedCallbacks;

    public NotifyPropertyChanged()
    {
        PropertyChangedCallbacks = new();
    }

    public event PropertyChangedEventHandler? PropertyChanged;


    protected virtual void Set<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
        => Set(ref backingField, value, EqualityComparer<T>.Default, propertyName);

    protected virtual void Set<T>(ref T backingField, T value, IEqualityComparer<T> comparer, [CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(comparer);
        ArgumentNullException.ThrowIfNull(propertyName);

        if (comparer.Equals(backingField, value))
        {
            return;
        }
        OnPropertyChanging(propertyName, backingField, value);
        backingField = value;
        OnPropertyChanged(propertyName, value);
    }

    protected virtual void OnPropertyChanging(string propertyName, object? oldValue, object? newValue)
    {

    }

    protected virtual void OnPropertyChanged(string propertyName, object? newValue)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
       CallPropertyChangedCallBacks(propertyName, newValue);
    }

    protected virtual void CallPropertyChangedCallBacks(string propertyName, object? newValue)
    {
        if (PropertyChangedCallbacks.TryGetValue(propertyName, out var callbacks))
        {
            foreach (var callback in callbacks)
            {
                callback(newValue);
            }
        }
    }

    protected virtual void AddCallBack<T>(T property,
        Action<T> callback,
        [CallerArgumentExpression(nameof(property))] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName);
        AddCallBack(callback, propertyName);
    }

    internal void AddCallBack<T>(Action<T?> callback, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(callback);
        ArgumentNullException.ThrowIfNull(propertyName);

        if (!PropertyChangedCallbacks.TryGetValue(propertyName, out var callbacks))
        {
            callbacks = new();
            PropertyChangedCallbacks.Add(propertyName, callbacks);
        }

        callbacks.Add(o =>
        {
            if(o is null)
            {
                callback(default);
            }
            if(o is not T value)
            {
                throw new InvalidCastException($"property '{propertyName}' can't be casted to {typeof(T)} ");
            }
            callback(value);
        });
    }
}
