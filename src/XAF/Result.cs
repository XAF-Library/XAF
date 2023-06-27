using System.Diagnostics.CodeAnalysis;

namespace XAF.Utilities;

public readonly struct Result<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;

    [MemberNotNullWhen(true, nameof(_exception))]
    [MemberNotNullWhen(false, nameof(_value))]
    public bool IsError => !IsSuccess;

    [MemberNotNullWhen(true, nameof(_value))]
    [MemberNotNullWhen(false, nameof(_exception))]
    
    public bool IsSuccess { get; }

    private Result(T value)
    {
        IsSuccess = true;
        _value = value;
    }

    private Result(Exception exception)
    {
        IsSuccess = false;
        _exception = exception;
    }

    public static implicit operator Result<T>(T value) => new(value);

    public static implicit operator Result<T>(Exception exception) => new(exception);

    public TResult Match<TResult>(
        Func<T, TResult> success,
        Func<Exception, TResult> failure)
        => IsSuccess ? success(_value) : failure(_exception);

    public void Switch(
        Action<T> success,
        Action<Exception> failure)
    {
        if (IsSuccess)
        {
            success(_value);
            return;
        }
            failure(_exception);
    }

}

public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;
    private readonly bool _success;

    [MemberNotNullWhen(true, nameof(_error))]
    [MemberNotNullWhen(false, nameof(_value))]
    public bool IsError => !_success;

    [MemberNotNullWhen(true, nameof(_value))]
    [MemberNotNullWhen(false, nameof(_error))]
    public bool IsSuccess => _success;

    private Result(TValue value)
    {
        _success = true;
        _value = value;
    }

    private Result(TError error)
    {
        _success = false;
        _error = error;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure)
        => IsSuccess ? success(_value) : failure(_error);

    public void Switch(
       Action<TValue> success,
       Action<TError> failure)
    {
        if (IsSuccess)
        {
            success(_value);
            return;
        }
        failure(_error);
    }
}
