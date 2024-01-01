using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAF.Modularity;
public interface IModuleStartup
{
    Task StartAsync(CancellationToken cancellationToken);
}
