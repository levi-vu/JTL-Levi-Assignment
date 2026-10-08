using FastEndpoints;
using Modules.User.Api;
using Modules.User.Api.Endpoints;
using Modules.WorkItems.Api;
using Modules.WorkItems.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddUserModule();
builder.Services.AddWorkItemsModule();
builder.Services.AddFastEndpoints(options =>
    options.Assemblies =
    [
        typeof(CreateUserEndpoint).Assembly,
        typeof(CreateWorkItemEndpoint).Assembly
    ]);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseFastEndpoints();

app.Run();