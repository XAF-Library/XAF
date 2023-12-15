using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ExtensionMethods;
using XAF.UI.WPF.ViewAdapters;
using XAF.UI.WPF.ViewComposition.Internal;

namespace XAF.UI.WPF.Hosting.Internal;
internal class WpfThread : IWpfThread
{
    private readonly ManualResetEvent _lock;
    private readonly ManualResetEvent _appStartedEventlock;
    private readonly ManualResetEvent _appCreatedEventlock;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _applicationLifetime;

    private bool _externalStop;

    public Thread Thread { get; }
    public Application? Application { get; private set; }

    public bool AppIsRunning { get; private set; }

    [MemberNotNullWhen(true, nameof(Application))]
    [MemberNotNullWhen(true, nameof(UiDispatcher))]
    public bool AppCreated { get; private set; }
    public Dispatcher? UiDispatcher { get; private set; }
    public Window? SplashWindow { get; set; }

    public WpfThread(IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
    {
        _applicationLifetime = applicationLifetime;
        _serviceProvider = serviceProvider;
        _lock = new ManualResetEvent(false);
        _appStartedEventlock = new(false);
        _appCreatedEventlock = new(false);

        ConfigureApp();

        Thread = new Thread(InternalThread)
        {
            IsBackground = false,
            Name = "Wpf Thread"
        };
        Thread.SetApartmentState(ApartmentState.STA);
        Thread.Start();
    }

    private void ConfigureApp()
    {
        InternalServiceProvider.Current = _serviceProvider;
        var metadataCollection = _serviceProvider.GetRequiredService<IBundleMetadataCollection>();
        metadataCollection.AddFromAssembly(Assembly.GetEntryAssembly());

        var viewAdapterCollection = _serviceProvider.GetRequiredService<IViewAdapterCollection>();
        viewAdapterCollection.AddAdapter<ContentControlAdapter>();
        viewAdapterCollection.AddAdapter<SelectorAdapter>();
    }

    private void InternalThread()
    {
        UiDispatcher = Dispatcher.CurrentDispatcher;
        var syncContext = new DispatcherSynchronizationContext(UiDispatcher);
        SynchronizationContext.SetSynchronizationContext(syncContext);
        var scheduler = new SynchronizationContextScheduler(syncContext);
        Schedulers.SetMainScheduler(scheduler);

        Application = _serviceProvider.GetRequiredService<Application>();
        AppCreated = true;
        _appCreatedEventlock.Set();
        _lock.WaitOne();
        if (_applicationLifetime.ApplicationStopped.IsCancellationRequested)
        {
            return;
        }

        AppIsRunning = true;

        _appStartedEventlock.Set();

        Application.Run();

        AppIsRunning = false;

        if (!_externalStop)
        {
            _applicationLifetime.StopApplication();
        }
    }

    public Task WaitForAppStart()
    {
        _appStartedEventlock.WaitOne();
        return Task.CompletedTask;
    }

    public Task WaitForAppCreation()
    {
        _appCreatedEventlock.WaitOne();
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellation)
    {
        _externalStop = false;
        _lock.Set();
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellation)
    {
        if (!AppIsRunning)
        {
            _lock.Set();
        }

        if (!AppIsRunning || !AppCreated || UiDispatcher.HasShutdownFinished)
        {
            return;
        }

        _externalStop = true;
        await UiDispatcher.InvokeAsync(Application.Shutdown);
    }
}
