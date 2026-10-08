## Purpose

Defines observable behavior for creating, retrieving, and verifying users while maintaining unique usernames.

## ADDED Requirements

### Requirement: Create a user
The system SHALL trim the submitted username, require 1 to 100 characters, assign a user ID, and return the created user's ID and username.

#### Scenario: Valid username
- **WHEN** a client submits an available valid username
- **THEN** the system persists the user and returns HTTP 201 with the user's ID and username

#### Scenario: Invalid username
- **WHEN** a client submits a blank username or a username longer than 100 characters after trimming
- **THEN** the system returns HTTP 400 without creating a user

### Requirement: Enforce username uniqueness
The system SHALL enforce case-insensitive username uniqueness, including during concurrent requests.

#### Scenario: Username conflicts
- **WHEN** one or more requests use a username already claimed with any letter casing
- **THEN** no additional user is created and each conflicting request returns HTTP 409

### Requirement: Retrieve a user
The system SHALL retrieve a user by ID without exposing internal domain or persistence models.

#### Scenario: User lookup
- **WHEN** a client requests a user ID
- **THEN** the system returns HTTP 200 with the matching user's ID and username, or HTTP 404 when no user matches

### Requirement: Verify user existence
The User module SHALL expose a public query contract that accepts a user ID and returns only whether that user exists.

#### Scenario: Existence lookup
- **WHEN** another module submits a user ID through the public query contract
- **THEN** the User module returns true for an existing user and false otherwise
