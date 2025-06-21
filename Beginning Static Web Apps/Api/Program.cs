using Api;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StacyClouds.SwaAuth.Models;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.ConfigureFunctionsWebApplication();
builder.Services.AddScoped<IRoleProcessor, RoleProcessor>();
// Application Insights isn't enabled by default.See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
