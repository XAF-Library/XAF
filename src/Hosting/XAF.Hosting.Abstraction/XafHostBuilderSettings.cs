using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace XAF.Hosting.Abstraction;

/// <summary>
/// Several settings for the host builder
/// </summary>
/// <param name="Args">the application arguments</param>
/// <param name="Configuration">the application configuration</param>
/// <param name="EnvironmentName">the environment name</param>
/// <param name="ApplicationName">the application name</param>
/// <param name="ContentRootPath">the content root path</param>
/// <param name="DisableDefaults">a value that indicates whether the default services should be disabled or not</param>
public record XafHostBuilderSettings(
    string[]? Args,
    ConfigurationManager? Configuration,
    string? EnvironmentName,
    string? ApplicationName,
    string? ContentRootPath,
    bool DisableDefaults);