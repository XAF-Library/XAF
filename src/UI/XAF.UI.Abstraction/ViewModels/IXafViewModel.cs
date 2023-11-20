using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.Abstraction.ViewModels;
public interface IXafViewModel
{
    void Preload();
    Task Load();
    Task Unload();
}

public interface IXafViewModel<in TParameter>
{
    void Preload(TParameter parameter);
}