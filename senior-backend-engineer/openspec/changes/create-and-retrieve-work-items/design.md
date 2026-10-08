## Context

The WorkItems projects contain only project scaffolding. The Users module already exposes `UserExistsQuery` from `Modules.User.Contracts`, and the existing runtime uses module-owned EF Core InMemory databases with MediatR and FastEndpoints. See the proposal and `work-item-management` spec for behavior.

## Goals / Non-Goals

**Goals:** Keep WorkItem as the aggregate root, preserve one-way module dependencies, use projected no-tracking reads, and match the existing User module conventions.

**Non-Goals:** User lifecycle changes, reassignment or editing, cross-module transactions, a shared kernel, or exposing domain and EF entities as contracts.

## Decisions

- Model `WorkItem` as the aggregate root with `Id`, `Name`, `Description`, and `AssigneeId`. Store only the user ID; a domain navigation to `User` would couple aggregates and modules.
- Validate the assignee in the create command handler by sending the existing `UserExistsQuery` from `Modules.User.Contracts` before constructing and saving the aggregate. Direct references to Users Domain, Infrastructure, DbContext, repositories, or tables are forbidden.
- Treat a failed existence query as the explicit `UserNotFound` application result, which the endpoint maps to HTTP 404 and the message `User not found`. Validation failures remain HTTP 400.
- Use a WorkItem-specific aggregate repository for writes and a separate query service for projected `AsNoTracking` reads. Loading aggregates for the list query would add tracking and data that the response does not need.
- Use a WorkItems-owned EF Core InMemory database, consistent with the existing service. A shared database context was rejected because it would violate schema and module ownership even with an in-memory provider.
- Expose `POST /work-items` and `GET /work-items?assigneeId={userId}` through thin FastEndpoints. Retrieval does not perform a Users lookup; no matches, including an unknown user ID, produce HTTP 200 with an empty collection.
- Use explicit DTO mapping because the models are small and the field mapping is direct. AutoMapper configuration would add indirection without reducing meaningful repetition.
- Reuse the repository's existing package versions and module-specific validation pipeline rather than introducing a shared abstraction.

## Risks / Trade-offs

- [A user could be removed after the existence check] → Accept eventual referential consistency and avoid a cross-module transaction; user deletion is outside the current scope.
- [In-memory WorkItems are lost on restart] → Accept the same persistence trade-off already used by the Users module for this assignment.
- [Multiple FastEndpoints module registrations can replace assembly discovery] → Append the WorkItems endpoint assembly without dropping the Users endpoint assembly and cover host composition in validation.
- [Unbounded assignee queries could grow large] → Accept an unpaged response for the assignment's “retrieve all” requirement and keep the query projected and no-tracking.

## Migration Plan

No schema migration or data backfill is required. Register the WorkItems module in the host; rollback removes its registration and endpoints without changing Users data.

## Files to Modify

- **Add** `src/Modules/WorkItems/Modules.WorkItems.Domain/WorkItems/WorkItem.cs` — define the aggregate root and name invariant.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/Abstractions/IWorkItemRepository.cs` — define aggregate persistence operations.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/Abstractions/IWorkItemQueries.cs` — define assignee-filtered projected reads.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/Behaviors/ValidationBehavior.cs` — apply FluentValidation to WorkItems requests.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/WorkItemDto.cs` — define the application read model.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/CreateWorkItem/CreateWorkItemCommand.cs` — define create input and result statuses.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/CreateWorkItem/CreateWorkItemCommandHandler.cs` — verify the assignee and orchestrate aggregate persistence.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/CreateWorkItem/CreateWorkItemCommandValidator.cs` — validate create request values.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/GetWorkItemsByAssignee/GetWorkItemsByAssigneeQuery.cs` — define the assignee query.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Application/WorkItems/GetWorkItemsByAssignee/GetWorkItemsByAssigneeQueryHandler.cs` — execute the projected read.
- **Modify** `src/Modules/WorkItems/Modules.WorkItems.Application/Modules.WorkItems.Application.csproj` — reference `Modules.User.Contracts` while retaining existing application packages.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Infrastructure/Persistence/WorkItemsDbContext.cs` — own the WorkItems EF model.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Infrastructure/Persistence/WorkItemConfiguration.cs` — map the aggregate.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Infrastructure/Persistence/WorkItemRepository.cs` — implement aggregate persistence.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Infrastructure/Persistence/WorkItemQueries.cs` — implement filtered no-tracking projections.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Api/Endpoints/CreateWorkItemEndpoint.cs` — expose creation and map validation and user-not-found outcomes.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Api/Endpoints/GetWorkItemsByAssigneeEndpoint.cs` — expose retrieval by assignee.
- **Add** `src/Modules/WorkItems/Modules.WorkItems.Api/DependencyInjection.cs` — register WorkItems handlers, validation, persistence, and endpoints.
- **Modify** `src/Modules/WorkItems/Modules.WorkItems.Api/Modules.WorkItems.Api.csproj` — reference the repository-approved FastEndpoints package version.
- **Modify** `src/Web.Api/Program.cs` — register the WorkItems module without replacing Users endpoint discovery.
- **Add** `src/Web.Api/WorkItems.http` — document create, missing-assignee, and retrieval requests.
- **Add** `tests/Modules.WorkItems.UnitTests/Modules.WorkItems.UnitTests.csproj` — define the NUnit and NSubstitute test project using repository-approved versions.
- **Add** `tests/Modules.WorkItems.UnitTests/Domain/WorkItemTests.cs` — test aggregate creation and name invariants.
- **Add** `tests/Modules.WorkItems.UnitTests/Application/WorkItemHandlersTests.cs` — test create and retrieval orchestration, including the Users contract boundary.
- **Add** `tests/Modules.WorkItems.UnitTests/Infrastructure/WorkItemPersistenceTests.cs` — test persistence and assignee filtering.
- **Add** `tests/Modules.WorkItems.UnitTests/Architecture/ModuleBoundaryTests.cs` — verify WorkItems references Users Contracts but not Users Domain or Infrastructure.
- **Modify** `JTL-Levi-Assignment.slnx` — include the WorkItems test project.
