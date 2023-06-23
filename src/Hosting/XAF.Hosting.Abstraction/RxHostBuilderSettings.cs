﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace XAF.Hosting.Abstraction;
public record RxHostBuilderSettings(
    string[]? Args,
    ConfigurationManager? Configuration,
    string? EnvironmentName,
    string? ApplicationName,
    string? ContentRootPath,
    bool DisableDefaults);