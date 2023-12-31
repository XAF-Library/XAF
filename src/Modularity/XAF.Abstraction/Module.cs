﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Modularity.Abstraction;
public abstract class Module : IModule
{
    public virtual string GetName()
    {
        return GetType().FullName!;
    }

    public virtual Version GetVersion()
    {
        return GetType().Assembly.GetName().Version!;
    }

    public virtual Task RegisterModuleAsync(IModuleRegistrationContext context, CancellationToken cancellation)
    {
        RegisterServices(context.GetServiceCollection());
        return Task.CompletedTask;
    }

    public virtual Task StartModuleAsync(IServiceProvider services, CancellationToken cancellation)
    {
        return Task.CompletedTask;
    }

    protected virtual void RegisterServices(IServiceCollection services) { }
}
