namespace ProductManagement.Core.Options;

public class TokenOptions
{
    public string Issuer { get; set; }
    public List<string> Audience { get; set; }
    public int AccessTokenExpirationAsHour { get; set; }
    public int RefreshTokenExpirationAsHour { get; set; }
    public string SecurityKey { get; set; }
}