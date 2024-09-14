namespace ProductManagement.Core.DTOs.Token;

public class TokenResponseDto
{
    public TokenResponseDto(string accessToken, DateTime accessTokenExpiration, string refreshToken,
        DateTime refreshTokenExpiration)
    {
        AccessToken = accessToken;
        AccessTokenExpiration = accessTokenExpiration;
        RefreshToken = refreshToken;
        RefreshTokenExpiration = refreshTokenExpiration;
    }

    public string AccessToken { get; init; }
    public DateTime AccessTokenExpiration { get; init; }
    public string RefreshToken { get; init; }
    public DateTime RefreshTokenExpiration { get; init; }
}