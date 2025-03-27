using AuthControl.Application.Common;
using MediatR;

namespace AuthControl.Application.Queries.Users.GetAllUsers;

public sealed record GetAllUsersQuery : IRequest<Result<List<GetAllUserResponse>>>;