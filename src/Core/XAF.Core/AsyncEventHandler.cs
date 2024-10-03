namespace XAF.Core;

public delegate Task AsyncEventHandler(object sender, EventArgs eventArgs);

public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs eventArgs);

public static class AsyncEventHandlerExtensions
{

    public static AsyncEventHandler Async(EventHandler eventHandler)
        => (s, e) =>
        {
            eventHandler(s, e);
            return Task.CompletedTask;
        };

    public static AsyncEventHandler<TEventArgs> Async<TEventArgs>(EventHandler<TEventArgs> eventHandler)
        => (s, e) =>
        {
            eventHandler(s, e);
            return Task.CompletedTask;
        };


    public static Task InvokeAsync(this AsyncEventHandler eventHandler, object sender, EventArgs args)
    {
        if (eventHandler == null)
        {
            return Task.CompletedTask;
        }

        var delegates = eventHandler.GetInvocationList().Cast<AsyncEventHandler>();
        var tasks = delegates.Select(t => t.Invoke(sender, args));


        return Task.WhenAll(tasks);
    }

    public static Task InvokeAsync<TEventArgs>(
        this AsyncEventHandler<TEventArgs> eventHandler,
        object sender,
        TEventArgs args)
    {
        if (eventHandler == null)
        {
            return Task.CompletedTask;
        }

        var delegates = eventHandler.GetInvocationList().Cast<AsyncEventHandler<TEventArgs>>();
        var tasks = delegates.Select(t => t.Invoke(sender, args));


        return Task.WhenAll(tasks);
    }
