# AGENTS.md

## Commands

Run these before considering a change complete:

```bash
dotnet restore
dotnet build
dotnet test
```

---

## Project

C# Modular Monolith using:

- Domain-Driven Design (DDD)
- CQRS with MediatR
- FastEndpoints
- Entity Framework Core
- SQL Server
- FluentValidation
- AutoMapper
- Domain Events.

Each business module owns its domain, application logic, persistence configuration, public contracts, and SQL schema.

---

## How to Work

Before changing code:

1. Identify the module that owns the behavior.
2. Inspect nearby code and follow existing patterns.
3. Read relevant architecture docs / ADRs if they exist.
4. Make the smallest change that satisfies the requirement.
5. Add or update tests for changed behavior.
6. Run the validation commands above.

Prefer existing conventions over introducing new abstractions.

---

## Module Boundaries

### Always

- Keep module boundaries strict.
- Keep business rules inside the owning module.
- Communicate across modules through explicit public contracts or integration events.
- Keep Controllers thin.
- Keep Domain independent from Application, Infrastructure, and API.
- Keep Application independent from Infrastructure implementations.
- Let Infrastructure implement persistence and external integrations.
- Keep each module responsible for its own SQL schema.

### Ask First

Ask before:

- adding a new NuGet dependency
- creating a new shared abstraction used by multiple modules
- introducing a new architectural pattern
- changing a public module contract
- changing database ownership between modules
- changing transaction boundaries
- modifying CI/CD or repository-wide build configuration
- adding a SharedKernel

### Never

Never:

- access another module's `DbContext`
- access another module's repositories
- reference another module's internal Domain or Infrastructure types
- query another module's SQL tables directly
- put business rules in Controllers
- introduce a generic `IRepository<T>`
- expose EF Core entities or Domain entities as API contracts
- use distributed transactions across modules for convenience
- disable or delete tests just to make a change pass
- commit secrets, credentials, connection strings, or API keys

---

## DDD

Use DDD for meaningful business behavior, not as ceremony.

- Aggregates enforce invariants.
- Change Aggregate state through behavior, not public setters.
- Repositories are for Aggregate Roots only.
- Use Value Objects when value semantics and invariants matter.
- Use Domain Services only when domain behavior does not naturally belong to an Entity or Aggregate.
- Domain Events remain internal to the module.

Prefer:

```csharp
order.Cancel();
```

over:

```csharp
order.Status = OrderStatus.Cancelled;
```

Do not force DDD abstractions onto simple CRUD without a domain reason.

---

## CQRS

### Commands

Commands change state.

- Handle commands through MediatR.
- Command handlers orchestrate the use case.
- Put business invariants in the Domain.
- Persist Aggregate changes through Aggregate-specific repositories.

Prefer:

```csharp
var order = await orderRepository.GetByIdAsync(
    command.OrderId,
    cancellationToken);

order.Cancel();
```

Avoid duplicating the `Cancel` business rules inside the handler.

### Queries

Queries never change state.

Use EF Core directly for read models when appropriate.

Prefer:

```csharp
var order = await dbContext.Orders
    .AsNoTracking()
    .Where(x => x.Id == query.OrderId)
    .Select(x => new OrderDto(
        x.Id,
        x.Number,
        x.Status))
    .SingleOrDefaultAsync(cancellationToken);
```

For queries:

- prefer `AsNoTracking()`
- prefer projection
- select only required columns
- avoid loading Aggregates only to create DTOs
- avoid unnecessary `Include()`

---

## Validation and Pipeline

Use FluentValidation for request/application validation.

Use MediatR pipeline behaviors for cross-cutting concerns where appropriate:

- validation
- logging
- transaction handling
- performance monitoring

FluentValidation does not replace Domain invariants.

---

## Mapping

Use AutoMapper for application-layer mapping when it reduces repetitive mapping code.

- Keep mapping profiles inside the owning module's Application layer.
- Map to explicit DTOs or public contracts; never expose Domain or EF Core entities.
- Prefer explicit mapping when it makes important business or transformation rules clearer.
- Supply any AutoMapper license through secure configuration or environment variables; never commit the key.

---

## Testing

For changed behavior:

- add/update Domain unit tests
- add integration tests for persistence or API flows when needed
- add regression tests for bug fixes
- preserve Architecture Tests that enforce dependency and module boundaries

Test conventions:

- use NUnit for unit tests
- use NSubstitute for mocking dependencies
- create the test fixture's mock services together in its constructor
- use NUnit's instance-per-test-case fixture lifecycle when constructor-created substitutes require isolation

Architecture Tests should catch rules such as:

```text
Domain !-> Infrastructure
Application !-> Infrastructure implementation
Module A !-> Module B internals
```

Do not weaken architecture tests to accommodate an implementation shortcut.

---

## Change Discipline

Keep changes focused.

Do not:

- refactor unrelated code
- rename unrelated files
- move module ownership without an architectural decision
- add abstractions "for future use"
- duplicate an existing pattern with a new pattern
- modify public contracts silently

If an architecture rule conflicts with the requested change, surface the conflict before implementing the architectural exception.

---

## Completion

Before finishing:

- correct module owns the change
- module boundaries remain intact
- business rules are in the correct layer
- build succeeds
- relevant tests pass
- architecture tests pass
- no secrets were introduced
- concurrency / transaction implications were considered where relevant
- summarize important trade-offs or follow-up risks

---

Do not invent architectural decisions when the repository does not provide enough information.
