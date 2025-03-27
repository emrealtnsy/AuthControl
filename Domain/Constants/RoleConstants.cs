namespace AuthControl.Domain.Constants;

public static class RoleConstants
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Customer = "Customer";
    
    public static readonly string[] Roles = [Admin, Manager, Customer];
}