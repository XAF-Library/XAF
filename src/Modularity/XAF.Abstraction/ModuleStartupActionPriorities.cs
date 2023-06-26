namespace XAF.Modularity.Abstraction;

public static class ModuleStartupActionPriorities
{
    public const int ModuleInitialization = -100;
    public const int BeforeModuleInitialization = ModuleInitialization - 10;
    public const int AfterModuleInitialization = ModuleInitialization + 10;
}
