using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using XAF.Modularity;
using XAF.UI.Abstraction;
using XAF.UI.Abstraction.Attributes;
using XAF.UI.Abstraction.ViewComposition;
using XAF.UI.Abstraction.ViewModels;
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
    private readonly ManualResetEvent _startupWindowLock = new ManualResetEvent(false);
    private readonly ILogger _logger;


    private Thread _wpfThread;
    private bool _appIsRunning;
    private bool _serviceStarted;
    private bool _outsideStop;
    private StartupViewModel? _startupVm;
    private Window? _startupWindow;
    private IBundleMetadata? _startupMetadata;

    public WpfService(
        IServiceProvider serviceProvider,
        IApplicationLifetime applicationLifetime,
        ILogger<WpfService> logger)
    {
        _serviceProvider = serviceProvider;
        _applicationLifetime = applicationLifetime;
        _wpfEnvironment = _serviceProvider.GetRequiredService<WpfEnvironment>();
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
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
        sw.Stop();
        _logger.LogInformation("Startup time {time}ms", sw.ElapsedMilliseconds);
        var moduleStartups = _serviceProvider.GetServices<IModuleStartup>();

        if (_startupWindow is null)
        {
            foreach (var moduleStartup in moduleStartups)
            {
                try
                {
                    await moduleStartup.PrepareAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Could not prepare module {moduleStartup}");
                }
            }

            await windowService.PrepareShells().ConfigureAwait(false);

            foreach (var moduleStartup in moduleStartups)
            {
                try
                {
                    await moduleStartup.StartAsync(cancellationToken).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Could not prepare module {moduleStartup}");
                }
            }

            await windowService.ShowShells().ConfigureAwait(false);

            return;
        }

        _startupWindowLock.WaitOne();

        _startupVm = (StartupViewModel)ActivatorUtilities.CreateInstance(_serviceProvider, _startupMetadata!.ViewModelType);
        _wpfEnvironment.WpfDispatcher.Invoke(() => _startupWindow.DataContext =  _startupVm);
        var startups = _serviceProvider.GetServices<IModuleStartup>();
        await _startupVm.PrepareAppAsync(startups, cancellationToken).ConfigureAwait(false);

        await windowService.PrepareShells().ConfigureAwait(false);

        await _startupVm.LoadAppAsync(startups, cancellationToken).ConfigureAwait(false);

        await windowService.ShowShells().ConfigureAwait(false);

        _wpfEnvironment.WpfDispatcher.Invoke(_startupWindow!.Close);

        await windowService.LoadShells().ConfigureAwait(false);
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
        var bundleMetadata = _serviceProvider.GetService<IBundleMetadataCollection>();

        _startupMetadata = bundleMetadata.GetMetadataForDecorator<StartupWindowAttribute>().FirstOrDefault();
        
        if (_startupMetadata != null)
        {
            var view = ActivatorUtilities.CreateInstance(_serviceProvider, _startupMetadata.ViewType);
            _startupWindow = view as Window;
        }

        _wpfEnvironment.AppStartLock.Set();
        _appIsRunning = true;

        void startupWindowActivated(object? sender, EventArgs e)
        {
            _startupWindow.Activated -= startupWindowActivated;
            _startupWindowLock.Set();
        }

        if (_startupWindow is not null)
        {
            _startupWindow.Activated += startupWindowActivated;
            app.Run(_startupWindow);
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
