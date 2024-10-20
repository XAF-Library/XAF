namespace XAF.Core.Modularity;
public interface IModuleCatalog
{
    Task Initialize();

    bool IsInitialized { get; }

    IEnumerable<Module> GetModules();

    Module Get(string name, Version version);
}
