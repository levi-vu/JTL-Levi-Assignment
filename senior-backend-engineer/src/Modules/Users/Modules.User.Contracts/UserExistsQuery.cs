using MediatR;

namespace Modules.User.Contracts;

public sealed record UserExistsQuery(Guid UserId) : IRequest<bool>;
