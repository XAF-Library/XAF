using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.Utilities;

namespace XAF.UI.Abstraction.ViewModels;
public abstract class XafViewModel : NotifyPropertyChanged, IXafViewModel
{
    private readonly SemaphoreSlim _waitForClose = new SemaphoreSlim(0);
    public virtual void Prepare() { }

    public virtual Task LoadAsync()
    {
        return Task.CompletedTask;
    }

    public virtual Task UnloadAsync()
    {
        _waitForClose.Release();
        return Task.CompletedTask;
    }

    public virtual int CompareTo(IXafViewModel? other)
    {
        return 0;
    }

    public Task WaitForViewClose()
    {
        return _waitForClose.WaitAsync();
    }
}

public abstract class XafViewModel<TParameter> : XafViewModel, IXafViewModel<TParameter>
{
    public abstract void Prepare(TParameter parameter);
}
