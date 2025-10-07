using DotNetEnv;
using Api.Extensions;

Env.Load("../../../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.Bootstrap(builder.Configuration);

var app = builder.Build();

app.ConfigurePipeline();

app.Run();
