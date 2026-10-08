## 1. Project Setup

- [ ] 1.1 `src/Modules/WorkItems/Modules.WorkItems.Application/Modules.WorkItems.Application.csproj`, `src/Modules/WorkItems/Modules.WorkItems.Api/Modules.WorkItems.Api.csproj` — add only the required Users Contracts project reference and repository-approved FastEndpoints package version, then verify `dotnet restore` succeeds.
- [ ] 1.2 `tests/Modules.WorkItems.UnitTests/Modules.WorkItems.UnitTests.csproj`, `JTL-Levi-Assignment.slnx` — add the NUnit/NSubstitute WorkItems test project with references to WorkItems layers and verify it appears in `dotnet test --list-tests` after tests are added.

## 2. Domain and Application

- [ ] 2.1 `src/Modules/WorkItems/Modules.WorkItems.Domain/WorkItems/WorkItem.cs`, `tests/Modules.WorkItems.UnitTests/Domain/WorkItemTests.cs` — implement the aggregate root with ID, name, description, and assignee ID; verify creation, trimming, and blank-name rejection with domain tests.
- [ ] 2.2 `src/Modules/WorkItems/Modules.WorkItems.Application/Abstractions/IWorkItemRepository.cs`, `src/Modules/WorkItems/Modules.WorkItems.Application/Abstractions/IWorkItemQueries.cs`, `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/WorkItemDto.cs`, `src/Modules/WorkItems/Modules.WorkItems.Application/Behaviors/ValidationBehavior.cs` — define WorkItem-specific persistence seams, explicit read models, and validation behavior; verify Application references no Infrastructure project.
- [ ] 2.3 `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/CreateWorkItem/CreateWorkItemCommand.cs`, `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/CreateWorkItem/CreateWorkItemCommandHandler.cs`, `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/CreateWorkItem/CreateWorkItemCommandValidator.cs`, `tests/Modules.WorkItems.UnitTests/Application/WorkItemHandlersTests.cs` — implement creation through `Modules.User.Contracts.UserExistsQuery` and verify created, invalid-name, and user-not-found paths, including that missing users are never persisted.
- [ ] 2.4 `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/GetWorkItemsByAssignee/GetWorkItemsByAssigneeQuery.cs`, `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/GetWorkItemsByAssignee/GetWorkItemsByAssigneeQueryHandler.cs`, `tests/Modules.WorkItems.UnitTests/Application/WorkItemHandlersTests.cs` — implement retrieval orchestration and verify matching and empty result collections.

## 3. Persistence and API

- [ ] 3.1 `src/Modules/WorkItems/Modules.WorkItems.Infrastructure/Persistence/WorkItemsDbContext.cs`, `src/Modules/WorkItems/Modules.WorkItems.Infrastructure/Persistence/WorkItemConfiguration.cs`, `src/Modules/WorkItems/Modules.WorkItems.Infrastructure/Persistence/WorkItemRepository.cs`, `src/Modules/WorkItems/Modules.WorkItems.Infrastructure/Persistence/WorkItemQueries.cs`, `tests/Modules.WorkItems.UnitTests/Infrastructure/WorkItemPersistenceTests.cs` — implement WorkItems-owned EF InMemory persistence and no-tracking assignee projections; verify saved fields, filtering, multiple matches, and empty results.
- [ ] 3.2 `src/Modules/WorkItems/Modules.WorkItems.Api/Endpoints/CreateWorkItemEndpoint.cs`, `src/Modules/WorkItems/Modules.WorkItems.Api/Endpoints/GetWorkItemsByAssigneeEndpoint.cs`, `src/Modules/WorkItems/Modules.WorkItems.Api/DependencyInjection.cs`, `src/Web.Api/Program.cs`, `src/Web.Api/WorkItems.http` — register the module and expose `POST /work-items` plus `GET /work-items?assigneeId={userId}` without hiding Users endpoints; verify documented requests produce 201, 400, 404 `User not found`, populated 200, and empty 200 outcomes.
- [ ] 3.3 `tests/Modules.WorkItems.UnitTests/Architecture/ModuleBoundaryTests.cs` — assert WorkItems Application depends on Users Contracts only and no WorkItems project references Users Domain or Infrastructure; verify the architecture test fails for a forbidden reference and passes for the intended graph.

## 4. Validation

- [ ] 4.1 `JTL-Levi-Assignment.slnx` — run `dotnet restore`, `dotnet build`, and `dotnet test`; verify all commands succeed, all User tests remain green, and no secrets or cross-module persistence access were introduced.
