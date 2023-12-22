using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.ViewComposition.Internal;

namespace XAF.UI;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultUiServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IViewAdapterCollection, ViewAdapterCollection>();
        services.TryAddSingleton<IViewService, ViewService>();
        services.TryAddSingleton<IBundleMetadataCollection, BundleMetadataCollection>();
        services.TryAddSingleton<INavigationService, NavigationService>();

        return services;
    }
}
