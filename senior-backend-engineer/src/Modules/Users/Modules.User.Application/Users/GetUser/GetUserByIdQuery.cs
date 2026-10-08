using MediatR;

namespace Modules.User.Application.Users.GetUser;

public sealed record GetUserByIdQuery(Guid UserId) : IRequest<UserDto?>;
