﻿using Microsoft.Extensions.Hosting;

namespace XAF.UI.WPF.Hosting;
public class WpfLifetime : IHostLifetime
{
    private readonly IWpfThread _thread;

    public WpfLifetime(IWpfThread wpfThread)
    {
        _thread = wpfThread;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _thread.StopAsync(cancellationToken);
    }

    public Task WaitForStartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
