## 1. Setup

- [x] 1.1 `src/Modules/Users/Modules.User.Contracts/Modules.User.Contracts.csproj`, `src/Modules/Users/Modules.User.Application/Modules.User.Application.csproj`, `src/Modules/Users/Modules.User.Infrastructure/Modules.User.Infrastructure.csproj`, `src/Modules/Users/Modules.User.Api/Modules.User.Api.csproj` — use the approved dependencies, including EF Core InMemory instead of SQL Server, and verify `dotnet restore` succeeds.
- [x] 1.2 `tests/Modules.User.UnitTests/Modules.User.UnitTests.csproj`, `JTL-Levi-Assignment.slnx` — add the NUnit and NSubstitute test project and verify it is discovered by `dotnet test --list-tests` after tests exist.

## 2. Domain and Application

- [x] 2.1 `src/Modules/Users/Modules.User.Domain/Users/User.cs`, `src/Modules/Users/Modules.User.Domain/Users/Username.cs`, `tests/Modules.User.UnitTests/Domain/UsernameTests.cs` — implement the aggregate and username invariants, verifying trimming, normalization, blank, overlong, and valid cases with unit tests.
- [x] 2.2 `src/Modules/Users/Modules.User.Application/Abstractions/IUserRepository.cs`, `src/Modules/Users/Modules.User.Application/Abstractions/IUserQueries.cs`, `src/Modules/Users/Modules.User.Application/Users/UserDto.cs` — define User-specific persistence seams and explicit read models, then verify Application has no Infrastructure reference.
- [x] 2.3 `src/Modules/Users/Modules.User.Application/Users/CreateUser/CreateUserCommand.cs`, `src/Modules/Users/Modules.User.Application/Users/CreateUser/CreateUserCommandHandler.cs`, `src/Modules/Users/Modules.User.Application/Users/CreateUser/CreateUserCommandValidator.cs`, `tests/Modules.User.UnitTests/Application/UserHandlersTests.cs` — implement creation and verify success, validation, and duplicate outcomes with unit tests.
- [x] 2.4 `src/Modules/Users/Modules.User.Application/Users/GetUser/GetUserByIdQuery.cs`, `src/Modules/Users/Modules.User.Application/Users/GetUser/GetUserByIdQueryHandler.cs`, `src/Modules/Users/Modules.User.Contracts/UserExistsQuery.cs`, `src/Modules/Users/Modules.User.Application/Users/UserExists/UserExistsQueryHandler.cs`, `tests/Modules.User.UnitTests/Application/UserHandlersTests.cs` — implement retrieval and public existence queries, verifying found/not-found and true/false cases with unit tests.

## 3. Persistence and API

- [x] 3.1 `src/Modules/Users/Modules.User.Infrastructure/Persistence/UsersDbContext.cs`, `src/Modules/Users/Modules.User.Infrastructure/Persistence/UserConfiguration.cs`, `src/Modules/Users/Modules.User.Infrastructure/Persistence/UserRepository.cs`, `src/Modules/Users/Modules.User.Infrastructure/Persistence/UserQueries.cs` — implement User-owned EF InMemory persistence with atomic normalized-username uniqueness and no-tracking projected reads.
- [x] 3.2 `src/Modules/Users/Modules.User.Infrastructure/Persistence/Migrations/`, `src/Modules/Users/Modules.User.Infrastructure/Persistence/UsersDbContextFactory.cs`, `tests/Modules.User.UnitTests/Infrastructure/UserRepositoryTests.cs` — remove relational migration artifacts and verify case-insensitive and concurrent uniqueness with NUnit tests.
- [x] 3.3 `src/Modules/Users/Modules.User.Api/Endpoints/CreateUserEndpoint.cs`, `src/Modules/Users/Modules.User.Api/Endpoints/GetUserByIdEndpoint.cs`, `src/Modules/Users/Modules.User.Api/DependencyInjection.cs`, `src/Web.Api/Program.cs`, `src/Web.Api/Web.Api.http`, `src/Web.Api/Users.http` — register the in-memory User database and verify the documented requests map to 201/400/409 and 200/404 outcomes.

## 4. Validation

- [x] 4.1 `JTL-Levi-Assignment.slnx` — run `dotnet restore`, `dotnet build`, and `dotnet test`; verify all succeed and review references to confirm other modules can access Users only through `Modules.User.Contracts`.
