using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Core.DTOs.Token;
using ProductManagement.Core.Interfaces.Services;
using ProductManagement.Domain.Entities;
using TokenOptions = ProductManagement.Core.Options.TokenOptions;

namespace ProductManagement.Infrastructure.Services;

public class TokenService(
    UserManager<AppUser> userManager,
    IOptions<TokenOptions> tokenOptions)
    : ITokenService
{
    private readonly TokenOptions _tokenOptions = tokenOptions.Value;
    private readonly UserManager<AppUser> _userManager = userManager;

    public TokenResponseDto CreateToken(AppUser appUser)
    {
        // Parameters
        var accessTokenExpiration = DateTime.Now.AddHours(_tokenOptions.AccessTokenExpirationAsHour);
        var refreshTokenExpiration = DateTime.Now.AddHours(_tokenOptions.RefreshTokenExpirationAsHour);
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));


        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256Signature);

        // Create Token
        JwtSecurityToken jwtSecurityToken = new(
            _tokenOptions.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: GetClaims(appUser, _tokenOptions.Audience),
            signingCredentials: signingCredentials
        );

        //Return access and refresh tokens 
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        var accessToken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

        var refreshToken = CreateRefreshToken();

        return new TokenResponseDto(accessToken, accessTokenExpiration, refreshToken, refreshTokenExpiration);
    }

    public async Task<bool> ValidateTokenAsync(string accessToken)
    {
        // Parameters
        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = _tokenOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = _tokenOptions.Audience[0],
            ValidateLifetime = true,
            IssuerSigningKey = securityKey
        };

        // Validate Token
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

        var tokenValidationResult =
            await jwtSecurityTokenHandler.ValidateTokenAsync(accessToken, tokenValidationParameters);
        if (tokenValidationResult.IsValid)
            return true;
        return false;
    }

    // Helper Methods 
    private IEnumerable<Claim> GetClaims(AppUser appUser, List<string> audiences)
    {
        if (appUser.UserName is null || appUser.Email is null || appUser.Id.ToString() is "")
            throw new Exception("User object must have UserName, Email and Id properties");

        var userClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, appUser.Email),
            new(ClaimTypes.Name, appUser.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        userClaims.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

        return userClaims;
    }

    private string CreateRefreshToken()
    {
        var bytes = new byte[32];

        using var rnd = RandomNumberGenerator.Create();
        rnd.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }
}