using System.Reactive.Concurrency;

namespace XAF.UI.Abstraction;
public static class Schedulers
{
    public static IScheduler MainScheduler { get; set; } = DefaultScheduler.Instance;

    public static IScheduler TaskPoolScheduler => System.Reactive.Concurrency.TaskPoolScheduler.Default;
}
