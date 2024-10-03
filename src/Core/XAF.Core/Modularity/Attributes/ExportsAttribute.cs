namespace XAF.Core.Modularity.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class ExportsAttribute<TService> : Attribute
    where TService : class
{
}
