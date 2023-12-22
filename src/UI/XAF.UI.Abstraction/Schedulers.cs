using System.Reactive.Concurrency;

namespace XAF.UI.Abstraction;
public static class Schedulers
{
    public static IScheduler MainScheduler { get; private set; } = DefaultScheduler.Instance;

    public static IScheduler TaskPoolScheduler => System.Reactive.Concurrency.TaskPoolScheduler.Default;

    public static void SetMainScheduler(IScheduler scheduler)
    {
        MainScheduler = scheduler;
    }
}
