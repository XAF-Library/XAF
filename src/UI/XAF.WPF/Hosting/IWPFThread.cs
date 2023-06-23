using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace XAF.UI.WPF.Hosting;
public interface IWpfThread
{
    bool AppIsRunnning { get; }

    [MemberNotNullWhen(true, nameof(Application))]
    [MemberNotNullWhen(true, nameof(UiDispatcher))]
    [MemberNotNullWhen(true, nameof(UiSyncContext))]
    bool AppCreated { get; }

    Thread Thread { get; }

    Application? Application { get; }

    Window? MainWindow => Application?.MainWindow;

    Window? SplashWindow { get; set; }

    SynchronizationContext? UiSyncContext { get; }
    Dispatcher? UiDispatcher { get; }


    Task StartAsync(CancellationToken cancellation);
    Task StopAsync(CancellationToken cancellation);

    Task WaitForAppStart();

    [MemberNotNull(nameof(UiSyncContext))]
    Task WaitForAppCreation();
}
