namespace AuthControl.Application.Common.Models;

public sealed record AccountCreateRequestModel(
    string UserName, 
    string Email, 
    string Password,
    string Name, 
    string Surname,
    string Role);