using Microsoft.Extensions.Hosting;
using System.Reactive.Concurrency;

namespace XAF.UI.Reactive;
public static class HostExtensions
{
#pragma warning disable IDE0060 // Nicht verwendete Parameter entfernen
    public static void AddRx(this IHost host, SynchronizationContext mainSchedulerSyncContext)
#pragma warning restore IDE0060 // Nicht verwendete Parameter entfernen
    {
        Schedulers.MainScheduler = new SynchronizationContextScheduler(mainSchedulerSyncContext);
    }
}
