using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.WorkItems.Api.Endpoints;
using Modules.WorkItems.Application.Abstractions;
using Modules.WorkItems.Application.Behaviors;
using Modules.WorkItems.Application.WorkItems.CreateWorkItem;
using Modules.WorkItems.Infrastructure.Persistence;

namespace Modules.WorkItems.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddWorkItemsModule(this IServiceCollection services)
    {
        services.AddFastEndpoints(options =>
            options.Assemblies = [typeof(CreateWorkItemEndpoint).Assembly]);

        services.AddMediatR(options =>
            options.RegisterServicesFromAssemblyContaining<CreateWorkItemCommandHandler>());

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));
        services.AddScoped<IValidator<CreateWorkItemCommand>, CreateWorkItemCommandValidator>();
        services.AddScoped<IWorkItemRepository, WorkItemRepository>();
        services.AddScoped<IWorkItemQueries, WorkItemQueries>();

        services.AddDbContext<WorkItemsDbContext>(options =>
            options.UseInMemoryDatabase("WorkItems"));

        return services;
    }
}
