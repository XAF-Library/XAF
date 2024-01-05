using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using XAF.Modularity;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.WPF.ExtensionMethods;
using XAF.UI.WPF.ViewAdapters;
using XAF.UI.WPF.ViewComposition;
using XAF.UI.WPF.ViewComposition.Internal;

namespace XAF.UI.WPF.Hosting.Internal;
public class WpfService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IApplicationLifetime _applicationLifetime;
    private readonly WpfEnvironment _wpfEnvironment;

    private Thread _wpfThread;
    private bool _appIsRunning;
    private bool _serviceStarted;
    private bool _outsideStop;
    private SplashWindow? _splashWindow;

    public WpfService(
        IServiceProvider serviceProvider,
        IApplicationLifetime applicationLifetime)
    {
        _serviceProvider = serviceProvider;
        _applicationLifetime = applicationLifetime;
        _wpfEnvironment = _serviceProvider.GetRequiredService<WpfEnvironment>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _serviceStarted = true;
        ConfigureApp();
        _wpfThread = new Thread(WpfThread)
        {
            IsBackground = false,
            Name = "Wpf Thread",
        };

        var windowService = _serviceProvider.GetRequiredService<IWindowService>();

        _wpfThread.SetApartmentState(ApartmentState.STA);
        cancellationToken.ThrowIfCancellationRequested();
        _wpfThread.Start();
        await _wpfEnvironment.WaitForAppStart().ConfigureAwait(false);
        var moduleStartups = _serviceProvider.GetServices<IModuleStartup>();

        foreach (var moduleStartup in moduleStartups)
        {
            await moduleStartup.PrepareAsync(cancellationToken).ConfigureAwait(false);
        }

        await windowService.CreateShells().ConfigureAwait(false);

        foreach (var moduleStartup in moduleStartups)
        {
            await moduleStartup.StartAsync(cancellationToken).ConfigureAwait(false);
        }

        if (_splashWindow is not null)
        {
            await _splashWindow.OnAppLoadAsync().ConfigureAwait(false);
            _wpfEnvironment.WpfDispatcher.Invoke(_splashWindow.Close);
        }

        await windowService.ShowShells().ConfigureAwait(false);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (!_serviceStarted || !_appIsRunning)
        {
            return;
        }

        _outsideStop = true;

        await _wpfEnvironment.WaitForAppStart().ConfigureAwait(false);
        _wpfEnvironment.WpfApp!.Dispatcher.InvokeShutdown();
        await _wpfEnvironment.WaitForAppShutDown().ConfigureAwait(false);
    }

    private void ConfigureApp()
    {
        InternalServiceProvider.Current = _serviceProvider;

        var metadataCollection = _serviceProvider.GetRequiredService<IBundleMetadataCollection>();
        metadataCollection.AddFromAssembly(Assembly.GetEntryAssembly());

        var viewAdapterCollection = _serviceProvider.GetRequiredService<IViewAdapterCollection>();
        viewAdapterCollection.AddAdaptersFromAssembly(Assembly.GetAssembly(typeof(ContentControlAdapter)));
    }

    private void WpfThread()
    {
        var dispatcher = Dispatcher.CurrentDispatcher;
        _wpfEnvironment.WpfDispatcher = dispatcher;
        var syncContext = new DispatcherSynchronizationContext(dispatcher);
        SynchronizationContext.SetSynchronizationContext(syncContext);
        var scheduler = new SynchronizationContextScheduler(syncContext);
        Schedulers.SetMainScheduler(scheduler);

        var app = _serviceProvider.GetRequiredService<Application>();
        _wpfEnvironment.WpfApp = app;
        _splashWindow = _serviceProvider.GetService<SplashWindow>();

        _wpfEnvironment.AppStartLock.Set();
        _appIsRunning = true;
        if (_splashWindow is not null)
        {
            app.Run(_splashWindow);
        }
        else
        {
            app.Run();
        }

        _appIsRunning = false;
        _wpfEnvironment.AppShutDownLock.Set();

        if (!_outsideStop)
        {
            _applicationLifetime.StopApplication();
        }
    }
}
