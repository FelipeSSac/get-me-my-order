using DotNetEnv;
using Worker.Extensions;

Env.Load("../../../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Configure Worker URLs
var workerUrls = builder.Configuration["WORKER_URLS"] ?? "http://localhost:5001";
builder.WebHost.UseUrls(workerUrls);

builder.Services.Bootstrap(builder.Configuration);

var app = builder.Build();

app.ConfigurePipeline();

app.Run();
