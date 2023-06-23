using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace XAF.UI.WPF.Hosting;
public static class HostExtensions
{
    public static async Task<SynchronizationContext> GetUiSyncContext(this IHost host)
    {
        var wpfThread = host.Services.GetRequiredService<IWpfThread>();
        await wpfThread.WaitForAppCreation();
        return wpfThread.UiSyncContext;
    }
}
