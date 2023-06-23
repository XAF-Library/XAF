using System.Reactive.Concurrency;

namespace XAF.UI.Reactive;
public static class Schedulers
{
    public static IScheduler MainScheduler { get; internal set; }

    public static IScheduler TaskPoolScheduler => System.Reactive.Concurrency.TaskPoolScheduler.Default;

}
