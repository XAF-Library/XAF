using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction;
public interface IActivatableViewModel : IViewModel
{
    void OnActivated();
    void OnDeactivated();
}

public interface IActivatableViewModel<in TParameter> : IViewModel
{
    void OnActivated(TParameter parameter);
    void OnDeactivated();
}
