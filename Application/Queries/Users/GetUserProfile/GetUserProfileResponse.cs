namespace AuthControl.Application.Queries.Users.GetUserProfile;

public sealed record GetUserProfileResponse(
    string Name,
    string Surname,
    string UserName,
    string Email,
    IList<string> Roles);
