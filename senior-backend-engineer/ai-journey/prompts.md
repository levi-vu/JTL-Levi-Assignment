\### Feature 1 — User Create and Query

$openspec-explore



Explore how to implement user management in the existing C# modular monolith.



Requirements:

\- Create a user with POST /users.

\- Retrieve a user by ID with GET /users/{id}.

\- A username must be trimmed and contain between 1 and 100 characters.

\- Usernames must be unique case-insensitively, including during concurrent requests.

\- Expose a public UserExistsQuery contract so other modules can verify whether a user exists. 
- Follow AGENTS.md


### Feature 2 — Work Item Create and Q$openspec-explore



Explore how to implement work-item management in the existing C# modular monolith.



Requirements:

\- Create a work item with POST /work-items.

\- A work item contains an ID, name, description, and assignee user ID.

\- The name must not be blank.

\- The assignee must be verified through the Users module’s public UserExistsQuery contract via mediator before the work item is created.

\- Keep WorkItem as an Aggregate Root, referencing User only by ID.

\- Keep module boundaries clean; do not access Users.Domain or Users.Infrastructure directly.


