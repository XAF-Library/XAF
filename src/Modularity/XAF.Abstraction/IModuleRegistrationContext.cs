using XAF.Hosting.Abstraction;

namespace XAF.Modularity.Abstraction;

public interface IModuleRegistrationContext
{
    T Get<T>();
}

public interface IModuleRegistrationContextBuilder
{
    IModuleRegistrationContext Build(IXafHostBuilder hostBuilder);
}