using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace XAF.WPF.UI.Internal;
internal class DefaultViewModelPresenterFactory : IViewModelPresenterFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultViewModelPresenterFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IViewModelPresenter CreateViewModelPresenter(object key)
    {
        return new ViewModelPresenter(
            key,
            _serviceProvider.GetRequiredService<IViewAdapterLocator>(),
            _serviceProvider.GetRequiredService<IViewLocator>(),
            _serviceProvider.GetRequiredService<ILogger<ViewModelPresenter>>()
            );
    }
}
