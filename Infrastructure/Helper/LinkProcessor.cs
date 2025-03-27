using System.Security.Cryptography;
namespace AuthControl.Infrastructure.Helper;

public class LinkProcessor : ILinkProcessor
{
    private const int TokenSizeInBytes = 64;
    private static readonly string[] AllowedSchemes = [Uri.UriSchemeHttp, Uri.UriSchemeHttps];

    public string CreateLink(string route, string token)
    { 
        route = NormalizeRoute(route);
        return $"{route}/{token}";
    }
    
    public bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;

        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
               AllowedSchemes.Contains(uriResult.Scheme) &&
               !string.IsNullOrWhiteSpace(uriResult.Host);
    }
    
    public string GenerateToken()
    {
        var bytes = new byte[TokenSizeInBytes];
        RandomNumberGenerator.Fill(bytes);
    
        return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }

    private static string NormalizeRoute(string route) => route.Trim('/');
}