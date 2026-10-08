## Purpose

Defines observable behavior for creating work items assigned to existing users and retrieving work items by assignee.

## ADDED Requirements

### Requirement: Create a work item
The system SHALL accept a name, description, and assignee user ID, assign a work item ID, and return the created work item. The name MUST contain at least one non-whitespace character.

#### Scenario: Valid work item
- **WHEN** a client submits a valid name, description, and existing assignee user ID
- **THEN** the system persists the work item and returns HTTP 201 with its ID, name, description, and assignee user ID

#### Scenario: Invalid work item name
- **WHEN** a client submits a blank work item name
- **THEN** the system returns HTTP 400 without creating a work item

### Requirement: Verify the assignee before creation
The system SHALL verify the assignee through the Users module's public contract before persisting a work item and SHALL NOT access Users domain or infrastructure internals.

#### Scenario: Assignee does not exist
- **WHEN** a client submits a work item whose assignee user ID does not identify an existing user
- **THEN** the system returns HTTP 404 with a "User not found" error and does not create the work item

### Requirement: Retrieve work items by assignee
The system SHALL return all work items assigned to the specified user ID without exposing domain or persistence models.

#### Scenario: Matching work items exist
- **WHEN** a client requests work items for a user ID with assigned work items
- **THEN** the system returns HTTP 200 with every matching work item's ID, name, description, and assignee user ID

#### Scenario: No matching work items exist
- **WHEN** a client requests work items for a user ID with no assigned work items
- **THEN** the system returns HTTP 200 with an empty collection
