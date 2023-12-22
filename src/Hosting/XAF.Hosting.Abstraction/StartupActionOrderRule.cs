using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Hosting.Abstraction;

/// <summary>
/// An order rule for startup actions
/// </summary>
public readonly struct StartupActionOrderRule
{

    private readonly HashSet<Type> _after;
    private readonly HashSet<Type> _before;

    /// <summary>
    /// An order rule for startup actions
    /// </summary>
    public StartupActionOrderRule()
    {
        _after = new();
        _before = new();
    }

    /// <summary>
    /// Gets all dependencies that should be executed after the current action
    /// </summary>
    public IEnumerable<Type> After => _after;

    /// <summary>
    /// Gets all dependencies that should be executed before the current action
    /// </summary>
    public IEnumerable<Type> Before => _before;

    /// <summary>
    /// Creates an new instance of ordering rule
    /// </summary>
    /// <returns></returns>
    public static StartupActionOrderRule Create()
    {
        return new StartupActionOrderRule();
    }

    /// <summary>
    /// Add an <see cref="After"/> dependency
    /// </summary>
    /// <typeparam name="T">the type of the action that must be executed after the current action</typeparam>
    /// <returns></returns>
    public StartupActionOrderRule ExecuteAfter<T>()
        where T : IHostStartupAction
    {
        _after.Add(typeof(T));
        return this;
    }
    /// <summary>
    /// Add an <see cref="Before"/> dependency
    /// </summary>
    /// <typeparam name="T">the type of the action that must be executed before the current action</typeparam>
    /// <returns></returns>
    public StartupActionOrderRule ExecuteBefore<T>()
        where T : IHostStartupAction
    {
        _before.Add(typeof(T));
        return this;
    }
}
