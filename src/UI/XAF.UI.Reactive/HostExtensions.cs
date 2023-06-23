using Microsoft.Extensions.Hosting;
using System.Reactive.Concurrency;

namespace XAF.UI.Reactive;
public static class HostExtensions
{
#pragma warning disable RCS1175 // Unused 'this' parameter.
    public static void UseRx(this IHost host, SynchronizationContext mainSchedulerSyncContext)
#pragma warning restore RCS1175 // Unused 'this' parameter.
    {
        Schedulers.MainScheduler = new SynchronizationContextScheduler(mainSchedulerSyncContext);
    }
}
