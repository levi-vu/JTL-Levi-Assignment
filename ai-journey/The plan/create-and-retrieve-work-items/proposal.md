## Why

The WorkItems module is an empty shell and does not yet provide the assignment workflow required by the service. The module must create and query work items without crossing the Users module boundary.

## What Changes

- Create work items with a name, description, and assignee user ID.
- Reject creation with a user-not-found response when the assignee does not exist.
- Retrieve all work items assigned to a specified user ID.
- Expose HTTP endpoints for creation and assignee-based retrieval.
- Add tests for WorkItem business behavior, application flows, persistence, and module boundaries.

## Capabilities

### New Capabilities

- `work-item-management`: Create work items for existing users and retrieve work items by assignee.

### Modified Capabilities

None.

## Impact

- Affects all WorkItems module layers and Web API composition.
- Adds a WorkItems-to-Users public-contract dependency for assignee existence checks.
- Adds WorkItems-owned persistence and test coverage without exposing or accessing Users internals.
