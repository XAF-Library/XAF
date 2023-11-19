using System.Reactive.Concurrency;

namespace XAF.UI;
public static class Schedulers
{
    public static IScheduler MainScheduler { get; internal set; } = DefaultScheduler.Instance;

    public static IScheduler TaskPoolScheduler => System.Reactive.Concurrency.TaskPoolScheduler.Default;

}
