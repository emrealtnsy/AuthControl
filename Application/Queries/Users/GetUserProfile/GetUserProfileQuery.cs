using AuthControl.Application.Common;
using MediatR;

namespace AuthControl.Application.Queries.Users.GetUserProfile;

public sealed record GetUserProfileQuery : IRequest<Result<GetUserProfileResponse>>;