using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.Modularity.Abstraction;

namespace XAF.UI.Abstraction;
public static class UiStartupActionPriorities
{
    public const int BeforeSplashScreen = ModuleStartupActionPriorities.BeforeModuleInitialization - 100;
    public const int ShowSplashScreen = BeforeSplashScreen + 50;
    public const int AfterSplashScreen = ShowSplashScreen + 10;
    public const int ShowMainWindow = ModuleStartupActionPriorities.AfterModuleInitialization + 50;
}
