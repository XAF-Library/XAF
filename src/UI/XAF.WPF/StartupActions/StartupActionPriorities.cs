﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XAF.Modularity.StartupActions;

namespace XAF.WPF.StartupActions;
public class StartupActionPriority
{
    public const int BeforSplashScreen = ModuleStartupActionPriorities.BeforeModuleInitialization - 100;
    public const int ShowSplashScreen = BeforSplashScreen + 50;
    public const int AfterSplashScreen = ShowSplashScreen + 10;
    public const int ShowMainWindow = ModuleStartupActionPriorities.AfterModuleInitialization + 50;
}
