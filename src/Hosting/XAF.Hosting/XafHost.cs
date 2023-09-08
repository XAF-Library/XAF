using Microsoft.Extensions.Hosting;
using ReactiveFramework.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using XAF.Hosting.Abstraction;

namespace XAF.Hosting;
public static class XafHost
{
    public static IXafHostBuilder CreateDefaultBuilder(string[] args)
        => new XafHostBuilder(new XafHostBuilderSettings(
            args,
            null,
            null,
            null,
            null,
            false));

    public static IXafHostBuilder CreateDefaultBuilder(XafHostBuilderSettings settings)
        => new XafHostBuilder(settings);
}
