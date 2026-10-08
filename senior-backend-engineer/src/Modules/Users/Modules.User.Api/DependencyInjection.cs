using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Modules.User.Api.Endpoints;
using Modules.User.Application.Abstractions;
using Modules.User.Application.Users.CreateUser;
using Modules.User.Infrastructure.Persistence;

namespace Modules.User.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddUserModule(this IServiceCollection services)
    {
        services.AddFastEndpoints(options =>
            options.Assemblies = [typeof(CreateUserEndpoint).Assembly]);

        services.AddMediatR(options =>
            options.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>());

        services.AddScoped<IValidator<CreateUserCommand>, CreateUserCommandValidator>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserQueries, UserQueries>();

        services.AddDbContext<UsersDbContext>(options =>
            options.UseInMemoryDatabase("Users"));

        return services;
    }
}
