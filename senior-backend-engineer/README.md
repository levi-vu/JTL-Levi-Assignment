# Senior Backend Engineer Assignment

This solution is a modular monolith built with ASP.NET Core, FastEndpoints, MediatR, Entity Framework Core, and Inmemory-Database. The main boundary is the business capability: **Users** owns user identity and lookup, while **WorkItems** owns work-item creation and retrieval. Each module contains its own API, Application, Domain, and Infrastructure projects, including its own `DbContext` and database schema. This keeps business rules and persistence details local to the module while retaining the operational simplicity of a single deployable application. When one module needs information owned by another, it uses an explicit public contract rather than referencing the other module's domain, infrastructure, or tables.

HTTP requests enter through thin FastEndpoints endpoints and are dispatched through MediatR. Commands represent state changes: a command handler coordinates the use case, invokes behavior on the relevant aggregate, and persists it through an aggregate-specific repository. FluentValidation handles request validation, while domain objects remain responsible for business invariants. Queries are separate from commands and use dedicated query abstractions backed by EF Core read paths. They return explicit DTOs and avoid exposing domain or persistence entities, allowing reads to use projections and no-tracking queries without compromising the write model.

The main trade-off is that this structure introduces more projects and indirection than a simple layered CRUD application. In return, ownership is explicit, module coupling is constrained, and each capability can evolve more safely. A single process and database also make deployment and local consistency straightforward, although module boundaries must be enforced by convention and tests.

## With More Time

I would:

- Move FluentValidation into MediatR pipeline behaviors so validation is applied consistently before command and query handlers run.
- Use a real SQL Server database for integration testing instead of relying only on lightweight test substitutes.
- Add end-to-end integration tests covering the API, persistence, module wiring, and cross-module contracts.
- Use AutoMapper where it removes repetitive mapping. The current DTOs are simple, so explicit mapping is clearer for now and avoids unnecessary configuration.
- Add architecture tests to continuously enforce module and dependency boundaries.
- Add reliable domain-event publication, such as an outbox, if events need to cross process boundaries.
- Improve observability around command and query execution with structured logging, tracing, and metrics.
