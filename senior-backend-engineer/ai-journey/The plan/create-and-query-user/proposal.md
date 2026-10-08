## Why

The User module has no behavior for creating or finding users. It also needs a boundary-safe way for other modules to verify a referenced user ID.

## What Changes

- Create users with unique usernames.
- Retrieve users by ID.
- Expose HTTP endpoints for both operations.
- Expose a public query contract that reports whether a user ID exists.
- Add unit tests for User business logic.

## Capabilities

### New Capabilities

- `user-management`: Create, retrieve, and verify the existence of users while enforcing username uniqueness.

### Modified Capabilities

None.

## Impact

- Affects all User module layers and Web API composition.
- Adds API, persistence, mediator-contract, and test dependencies.
- Establishes a public User contract for cross-module existence checks.
