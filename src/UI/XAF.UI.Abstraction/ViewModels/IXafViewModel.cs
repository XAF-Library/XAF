using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction.ViewModels;
public interface IXafViewModel : IComparable<IXafViewModel>
{
    void Prepare();
    Task LoadAsync();
    Task UnloadAsync();
    Task WaitForViewClose();
}

public interface IXafViewModel<in TParameter> : IXafViewModel
{
    void Prepare(TParameter parameter);
}