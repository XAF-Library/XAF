namespace XAF.Core.Modularity;
internal interface IModuleHandler
{
    Task LoadAsync(object module);

    Task StartAsync(object module);
}
