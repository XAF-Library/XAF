using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Hosting.Internal;
internal class XafHostEnvironment : IHostEnvironment
{
    public XafHostEnvironment(
        string environmentName, 
        string applicationName, 
        string contentRootPath, 
        IFileProvider contentRootFileProvider)
    {
        EnvironmentName = environmentName;
        ApplicationName = applicationName;
        ContentRootPath = contentRootPath;
        ContentRootFileProvider = contentRootFileProvider;
    }

    public string EnvironmentName { get; set; }
    public string ApplicationName { get; set; }
    public string ContentRootPath { get; set; }
    public IFileProvider ContentRootFileProvider { get; set; }
}
