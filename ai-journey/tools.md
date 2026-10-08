# Tools and Skills

## AI Coding Agent

I used **OpenAI Codex** as the primary coding agent to inspect the existing codebase, clarify requirements, prepare the implementation plan, write code and tests, and validate the final solution.

## Spec-Driven Development

I used **OpenSpec** to define each feature before implementation. The generated proposals, specifications, designs, and task lists are available in [`ai-journey/The plan`](./The%20plan/).

The following OpenSpec skills were used:

- **`openspec-explore`** — investigated the existing architecture, clarified requirements, identified module boundaries, and discussed design trade-offs.
- **`openspec-propose`** — converted the explored requirements into a proposal, behavioral specification, technical design, and implementation tasks.
- **`openspec-apply-change`** — implemented the approved tasks incrementally while keeping the code aligned with the OpenSpec artifacts.

## Backend Development Guidance

- **`aspnet-core`** — guided the ASP.NET Core structure, dependency injection, FastEndpoints integration, MediatR request flow, EF Core persistence, and automated testing approach.
- **`coderabbit:code-review skill`**

## Development Tools

- **OpenSpec CLI** — inspected change status and managed the specification-driven workflow.
- **.NET CLI** — restored dependencies, built the solution, and ran the automated tests using `dotnet restore`, `dotnet build`, and `dotnet test`.
- **Ripgrep (`rg`)** — searched the repository and inspected existing implementation patterns quickly.

## Technologies Used
- .NET 8 and ASP.NET Core
- FastEndpoints
- MediatR
- Entity Framework Core InMemory
- FluentValidation
- NUnit
- NSubstitute

