using XAF.Hosting.Abstraction;
using XAF.Modularity.Abstraction;
using XAF.Modularity.Internal;

namespace XAF.Modularity;
public class ModuleRegistrationContextBuilder : IModuleRegistrationContextBuilder
{
    private readonly Dictionary<Type, object> _contextObjects = new();


    public IModuleRegistrationContext Build(IXafHostBuilder builder)
    {
        ProvideContextObjects(builder);
        return new ModuleRegistrationContext(_contextObjects);
    }

    protected virtual void ProvideContextObjects(IXafHostBuilder builder)
    {
        AddContextObject(builder.Services);
        AddContextObject(builder.Configuration);
        AddContextObject(builder.Environment);
        AddContextObject(builder.Logging);
    }

    protected void AddContextObject<T>(T ctxObject)
        where T : notnull
    {
        _contextObjects.Add(typeof(T), ctxObject);
    }
}
