using Microsoft.Extensions.DependencyInjection;
using XAF.Hosting.Abstraction;
using XAF.Modularity.Abstraction;
using XAF.Modularity.Abstraction.StartupActions;
using XAF.Modularity.Internal;

namespace XAF.Modularity.Extensions;
public static class HostBuilderExtensions
{
    public static void ConfigureModularity(this IXafHostBuilder builder)
    {
        builder.Services.AddStartupAction<StartModules>();
        builder.GetModuleCatalog();
    }

    public static void ConfigureModularity<TCatalog>(this IXafHostBuilder builder)
        where TCatalog : IModuleCatalog, new()
    {
        builder.Services.AddStartupAction<StartModules>();
        builder.Properties[typeof(IModuleCatalog)] = new TCatalog();
        builder.GetModuleCatalog();
    }

    public static void UseModuleRegistrationContextBuilder<T>(this IXafHostBuilder builder)
        where T : IModuleRegistrationContextBuilder, new()
    {
        builder.Properties[typeof(IModuleRegistrationContextBuilder)] = new T();
    }

    public static void UseModuleRegistrationContextBuilder<T>(this IXafHostBuilder builder, T ctxBuilder)
        where T : IModuleRegistrationContextBuilder
    {
        builder.Properties[typeof(IModuleRegistrationContextBuilder)] = ctxBuilder;
    }

    public static async Task RegisterModuleAsync<T>(this IXafHostBuilder builder, CancellationToken cancellation = default)
        where T : IModule, new()
    {
        var catalog = builder.GetModuleCatalog();
        var module = catalog.Add<T>();
        var ctx = builder.GetModuleRegistrationContext();
        await module.RegisterModuleAsync(ctx, cancellation)
            .ConfigureAwait(false);
    }

    private static IModuleCatalog GetModuleCatalog(this IXafHostBuilder builder)
    {
        if (!builder.Properties.TryGetValue(typeof(IModuleCatalog), out var moduleCatalog))
        {
            moduleCatalog = new ModuleCatalog();
            builder.Services.AddSingleton((IModuleCatalog)moduleCatalog);
            builder.Properties[typeof(IModuleCatalog)] = moduleCatalog;
        }

        return (IModuleCatalog)moduleCatalog;
    }

    private static IModuleRegistrationContext GetModuleRegistrationContext(this IXafHostBuilder builder)
    {

        if (!builder.Properties.TryGetValue("ContextBuilderBuilded", out var _))
        {
            builder.BuildModuleRegistrationContext();
        }
        return (IModuleRegistrationContext)builder.Properties[typeof(IModuleRegistrationContext)];
    }

    private static void BuildModuleRegistrationContext(this IXafHostBuilder builder)
    {
        if (!builder.Properties.TryGetValue(typeof(IModuleRegistrationContextBuilder), out var contextBuilder))
        {
            contextBuilder = new ModuleRegistrationContextBuilder();
            builder.Properties[typeof(IModuleRegistrationContextBuilder)] = contextBuilder;
        }
        builder.Properties[typeof(IModuleRegistrationContext)] = ((IModuleRegistrationContextBuilder)contextBuilder).Build(builder);
        builder.Properties["ContextBuilderBuilded"] = new object();
    }
}
