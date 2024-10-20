namespace XAF.Core.Modularity;
public interface IModuleManager
{
    Task Initialize();

    IEnumerable<Module> Modules { get; }

    Task LoadModules();

    Task StartModules();
}
