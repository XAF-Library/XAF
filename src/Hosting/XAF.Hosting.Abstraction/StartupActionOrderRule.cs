using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Hosting.Abstraction;
public readonly struct StartupActionOrderRule
{

    private readonly HashSet<Type> _after;
    private readonly HashSet<Type> _before;

    private StartupActionOrderRule(Type targetType)
    {
        _after = new();
        _before = new();
        TargetType = targetType;
    }

    public Type TargetType { get; }

    public IEnumerable<Type> After => _after;

    public IEnumerable<Type> Before => _before;

    public static StartupActionOrderRule CreateFor<T>()
        where T : IHostStartupAction
    {
        return new StartupActionOrderRule(typeof(T));
    }

    public StartupActionOrderRule ExecuteAfter<T>()
        where T : IHostStartupAction
    {
        _after.Add(typeof(T));
        return this;
    }

    public StartupActionOrderRule ExecuteBefore<T>()
        where T : IHostStartupAction
    {
        _before.Add(typeof(T));
        return this;
    }
}
