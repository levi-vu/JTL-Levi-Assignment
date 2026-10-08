# Senior Backend Engineer Assignment

An ASP.NET Core modular monolith for managing users and work items.

## Modules

- **Users**: creates users and retrieves user information.
- **WorkItems**: creates work items and finds them by assignee.

Each module owns its API, application logic, domain model, persistence, `DbContext`, and database schema. Modules communicate through public contracts instead of accessing each other's internals.

## Architecture

Requests follow this flow:

`FastEndpoints -> MediatR -> Application handler -> Domain/Repository`

- Commands change data.
- Queries return DTOs using no-tracking EF Core projections.
- FluentValidation validates requests.
- Domain objects enforce business rules.

## Potential Improvements

Given more time, I would consider the following improvements:

* **Implement Result Pattern and move FluentValidation into the MediatR pipeline:** Keep controllers thinner, cleaner, and easier to read.
* **Add integration tests:** Verify API endpoints and database interactions.
* **Handle concurrent user creation:** Prevent duplicate usernames when multiple requests attempt to create users simultaneously.

## Testing

I created a `test.http` file to manually test all API endpoints implemented in this assessment.

**File location:** `senior-backend-engineer/src/Web.Api/test.http`

  