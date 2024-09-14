using ProductManagement.Core.DTOs.Token;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Core.Interfaces.Services;

public interface ITokenService
{
    /// <summary>
    ///     Creates a new token for the given user
    /// </summary>
    /// <param name="appUser"></param>
    /// <returns></returns>
    TokenResponseDto CreateToken(AppUser appUser);

    /// <summary>
    ///     Validates the given token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    Task<bool> ValidateTokenAsync(string accessToken);
}