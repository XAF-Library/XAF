using System.Reflection;

namespace XAF.Core.Modularity;

public class Module
{
    public string Name { get; }

    public string Description { get; }

    public Version Version { get; }

    public Type Type { get; }

    public Assembly Assembly { get; }

    public IModuleCatalog Source { get; }

}