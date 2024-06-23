using System.Diagnostics.CodeAnalysis;
using System.Runtime.ExceptionServices;

namespace XAF.Utilities;

/// <summary>
/// A result return type
/// </summary>
/// <typeparam name="T">the type of the result</typeparam>
public readonly struct Result<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;

    [MemberNotNullWhen(true, nameof(_exception))]
    [MemberNotNullWhen(false, nameof(_value))]
    private bool IsError => !IsSuccess;

    [MemberNotNullWhen(true, nameof(_value))]
    [MemberNotNullWhen(false, nameof(_exception))]
    
    private bool IsSuccess { get; }

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

    /// <summary>
    /// Creates a new succeeded result
    /// </summary>
    /// <param name="value">the value</param>
    public static implicit operator Result<T>(T value) => new(value);


    /// <summary>
    /// Created a new failed result
    /// </summary>
    /// <param name="exception">the error message</param>
    public static implicit operator Result<T>(Exception exception) => new(exception);


    /// <summary>
    /// Return a value based on the result state
    /// </summary>
    /// <typeparam name="TResult">the result type</typeparam>
    /// <param name="success">a method that is called when the result succeeded</param>
    /// <param name="failure">a method that is called when the result fails</param>
    /// <returns></returns>
    public TResult Match<TResult>(
        Func<T, TResult> success,
        Func<Exception, TResult> failure)
        => IsSuccess ? success(_value) : failure(_exception);

    /// <summary>
    /// execute methods based on the result state
    /// </summary>
    /// <param name="success">a method that is called when the result succeeded</param>
    /// <param name="failure">a method that is called when the result fails</param>
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

/// <summary>
/// A result return type
/// </summary>
/// <typeparam name="TValue">the tye of the succeeded state</typeparam>
/// <typeparam name="TError">the type of the failed state</typeparam>
public readonly struct Result<TValue, TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;
    private readonly bool _success;

    [MemberNotNullWhen(true, nameof(_error))]
    [MemberNotNullWhen(false, nameof(_value))]
    private bool IsError => !_success;

    [MemberNotNullWhen(true, nameof(_value))]
    [MemberNotNullWhen(false, nameof(_error))]
    private bool IsSuccess => _success;

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

    /// <summary>
    /// Creates a new succeeded result
    /// </summary>
    /// <param name="value">the value</param>
    public static implicit operator Result<TValue, TError>(TValue value) => new(value);

    /// <summary>
    /// Created a new failed result
    /// </summary>
    /// <param name="error">the error</param>
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    /// <summary>
    /// Return a value based on the result state
    /// </summary>
    /// <typeparam name="TResult">the result type</typeparam>
    /// <param name="success">a method that is called when the result succeeded</param>
    /// <param name="failure">a method that is called when the result fails</param>
    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure)
        => IsSuccess ? success(_value) : failure(_error);

    /// <summary>
    /// execute methods based on the result state
    /// </summary>
    /// <param name="success">a method that is called when the result succeeded</param>
    /// <param name="failure">a method that is called when the result fails</param>
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
