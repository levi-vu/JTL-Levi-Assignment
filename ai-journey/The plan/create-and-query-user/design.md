## Context

The User projects are empty shells. The existing dependency direction is API → Application/Infrastructure, Infrastructure → Application, and Application → Domain. See the proposal and user-management spec for behavior.

## Goals / Non-Goals

**Goals:** Preserve module boundaries, keep endpoints thin, make uniqueness concurrency-safe, and support database-free unit tests.

**Non-Goals:** Authentication, user updates/deletion, WorkItems consumption, shared abstractions, or automatic migration at startup.

## Decisions

- Model `User` as the aggregate root and `Username` as a value object that owns trimming, length, display value, and invariant normalization.
- Use a User-specific write repository and projected no-tracking read service; avoid a generic repository and avoid returning domain entities.
- Use EF Core InMemory as the runtime store. Enforce normalized-username uniqueness inside an application-process critical section because the provider does not enforce unique indexes.
- Put `UserExistsQuery(Guid UserId) : IRequest<bool>` in User Contracts and its handler in User Application. Other modules reference Contracts only.
- Implement `POST /users` and `GET /users/{id:guid}` as FastEndpoints that delegate to MediatR and map explicit results to HTTP responses.
- Use explicit mapping because the DTOs are small. Use NUnit with NSubstitute for focused domain and handler tests.
- Use a module-owned in-memory database name and require no external database configuration.

## Risks / Trade-offs

- [Concurrent duplicate requests race in the InMemory provider] → Serialize the existence check and insert inside the repository.
- [Data is lost when the process stops] → Accept this as an explicit in-memory storage trade-off for the assignment.
- [Normalization differs by culture] → Normalize with invariant casing before persistence and lookup.
- [Contracts depend on MediatR] → Accept the coupling because MediatR queries are the requested module integration mechanism.

## Migration Plan

No schema migration is required. Deploying or restarting the host creates an empty in-memory User store.

## Files to Modify

- **Add** `src/Modules/Users/Modules.User.Domain/Users/User.cs` — define the aggregate root.
- **Add** `src/Modules/Users/Modules.User.Domain/Users/Username.cs` — define username invariants and normalization.
- **Add** `src/Modules/Users/Modules.User.Application/Abstractions/IUserRepository.cs` — define aggregate persistence operations.
- **Add** `src/Modules/Users/Modules.User.Application/Abstractions/IUserQueries.cs` — define projected read operations.
- **Add** `src/Modules/Users/Modules.User.Application/Users/UserDto.cs` — define the application read model.
- **Add** `src/Modules/Users/Modules.User.Application/Users/CreateUser/CreateUserCommand.cs` — define create input and outcome.
- **Add** `src/Modules/Users/Modules.User.Application/Users/CreateUser/CreateUserCommandHandler.cs` — orchestrate creation.
- **Add** `src/Modules/Users/Modules.User.Application/Users/CreateUser/CreateUserCommandValidator.cs` — validate command input.
- **Add** `src/Modules/Users/Modules.User.Application/Users/GetUser/GetUserByIdQuery.cs` — define retrieval input.
- **Add** `src/Modules/Users/Modules.User.Application/Users/GetUser/GetUserByIdQueryHandler.cs` — execute retrieval.
- **Add** `src/Modules/Users/Modules.User.Application/Users/UserExists/UserExistsQueryHandler.cs` — handle the public existence contract.
- **Add** `src/Modules/Users/Modules.User.Contracts/UserExistsQuery.cs` — expose the MediatR query contract.
- **Modify** `src/Modules/Users/Modules.User.Contracts/Modules.User.Contracts.csproj` — reference MediatR.
- **Modify** `src/Modules/Users/Modules.User.Application/Modules.User.Application.csproj` — reference User Contracts.
- **Add** `src/Modules/Users/Modules.User.Infrastructure/Persistence/UsersDbContext.cs` — own the User EF model.
- **Add** `src/Modules/Users/Modules.User.Infrastructure/Persistence/UserConfiguration.cs` — map the aggregate for EF Core InMemory.
- **Add** `src/Modules/Users/Modules.User.Infrastructure/Persistence/UserRepository.cs` — implement atomic in-memory writes and uniqueness checks.
- **Add** `src/Modules/Users/Modules.User.Infrastructure/Persistence/UserQueries.cs` — implement projected reads.
- **Modify** `src/Modules/Users/Modules.User.Infrastructure/Modules.User.Infrastructure.csproj` — use the EF Core InMemory provider and remove SQL Server dependencies.
- **Add** `src/Modules/Users/Modules.User.Api/Endpoints/CreateUserEndpoint.cs` — expose user creation.
- **Add** `src/Modules/Users/Modules.User.Api/Endpoints/GetUserByIdEndpoint.cs` — expose user retrieval.
- **Add** `src/Modules/Users/Modules.User.Api/DependencyInjection.cs` — register the User module.
- **Modify** `src/Modules/Users/Modules.User.Api/Modules.User.Api.csproj` — reference FastEndpoints.
- **Modify** `src/Web.Api/Program.cs` — register and map FastEndpoints and the User module.
- **Modify** `src/Web.Api/Web.Api.http` — add example requests.
- **Add** `tests/Modules.User.UnitTests/Modules.User.UnitTests.csproj` — define the NUnit and NSubstitute unit-test project.
- **Add** `tests/Modules.User.UnitTests/Domain/UsernameTests.cs` — test domain invariants.
- **Add** `tests/Modules.User.UnitTests/Application/UserHandlersTests.cs` — test command and query behavior.
- **Add** `tests/Modules.User.UnitTests/Infrastructure/UserRepositoryTests.cs` — test case-insensitive and concurrent uniqueness.
- **Modify** `JTL-Levi-Assignment.slnx` — include the unit-test project.
