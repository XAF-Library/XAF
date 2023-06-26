using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace XAF.Hosting.Abstraction;
public record XafHostBuilderSettings(
    string[]? Args,
    ConfigurationManager? Configuration,
    string? EnvironmentName,
    string? ApplicationName,
    string? ContentRootPath,
    bool DisableDefaults);