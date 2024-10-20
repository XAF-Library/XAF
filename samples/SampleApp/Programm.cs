using Microsoft.Extensions.Hosting;
using XAF.Core.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.UseXaf();
builder.Build();