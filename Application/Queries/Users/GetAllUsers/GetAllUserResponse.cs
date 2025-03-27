namespace AuthControl.Application.Queries.Users.GetAllUsers;

public sealed record GetAllUserResponse(
    string Name,
    string Surname,
    string UserName,
    string Email,
    bool EmailConfirmed, 
    IList<string> Roles);
