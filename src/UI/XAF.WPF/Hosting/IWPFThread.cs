﻿using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace XAF.UI.WPF.Hosting;
public interface IWpfThread
{
    bool AppIsRunning { get; }

    [MemberNotNullWhen(true, nameof(Application))]
    [MemberNotNullWhen(true, nameof(UiDispatcher))]
    bool AppCreated { get; }

    Thread Thread { get; }

    Application? Application { get; }

    Window? MainWindow => Application?.MainWindow;

    Window? SplashWindow { get; set; }
    Dispatcher? UiDispatcher { get; }

    Task StartAsync(CancellationToken cancellation);
    Task StopAsync(CancellationToken cancellation);

    Task WaitForAppStart();

    [MemberNotNullWhen(true, nameof(Application))]
    [MemberNotNull(nameof(UiDispatcher))]
    Task WaitForAppCreation();
}
