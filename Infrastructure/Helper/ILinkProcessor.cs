namespace AuthControl.Infrastructure.Helper;

public interface ILinkProcessor
{
    string CreateLink(string route, string token);
    bool IsValidUrl(string url);
    string GenerateToken();
}