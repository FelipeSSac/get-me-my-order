using DotNetEnv;
using Order.Infrastructure.IOC;
using Api.Extensions;

Env.Load("../../../../.env");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register validation
builder.Services.AddValidation();

// Register database
builder.Services.AddDatabase(builder.Configuration);

// Register repositories
builder.Services.AddRepositories();

// Register use cases
builder.Services.AddUseCases();

// Register Azure Service Bus
builder.Services.AddServiceBus(builder.Configuration);

// Register event infrastructure
builder.Services.AddEvents();

// Register health checks
builder.Services.AddApplicationHealthChecks(builder.Configuration);

var app = builder.Build();

app.ConfigurePipeline();

app.Run();
