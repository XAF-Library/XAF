using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XAF.Utilities;
public abstract class NotfiyPropertyChanged : INotifyPropertyChanged
{
    private readonly Dictionary<string, List<Action<object>>> _callbacks;

    public NotfiyPropertyChanged()
    {
        _callbacks = new();
    }

    public event PropertyChangedEventHandler? PropertyChanged;


    protected void Set<T>(ref T backingField, T value, [CallerMemberName] string? propertyName = null)
        => Set(ref backingField, value, EqualityComparer<T>.Default, propertyName);

    protected void Set<T>(ref T backingField, T value, IEqualityComparer<T> comparer, [CallerMemberName] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(comparer);
        ArgumentNullException.ThrowIfNull(propertyName);

        if (comparer.Equals(backingField, value))
        {
            return;
        }

        backingField = value;
        OnPropertyChanged(propertyName);

        if (_callbacks.TryGetValue(propertyName, out var callbacks))
        {
            foreach (var callback in callbacks)
            {
                callback(backingField);
            }
        }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void AddCallBack<T>(T property,
        Action<T> callback,
        [CallerArgumentExpression(nameof(property))] string? propertyName = null)
    {
        ArgumentNullException.ThrowIfNull(propertyName);
        AddCallBack(callback, propertyName);
    }

    internal void AddCallBack<T>(Action<T> callback, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(callback);
        ArgumentNullException.ThrowIfNull(propertyName);

        if (!_callbacks.TryGetValue(propertyName, out var callbacks))
        {
            callbacks = new();
            _callbacks.Add(propertyName, callbacks);
        }

        callbacks.Add(o => callback((T)o));
    }
}
