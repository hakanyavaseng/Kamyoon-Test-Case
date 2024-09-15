using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Core.DTOs.ApiResponses;
using ProductManagement.Core.DTOs.Token;
using ProductManagement.Core.Interfaces.Services;
using ProductManagement.Domain.Entities;

namespace ProductManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<AppUser> _userManager;


    public TokenController(ITokenService tokenService, UserManager<AppUser> userManager)
    {
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpGet("{name}")]
    public async Task<ApiResponse<TokenResponseDto>> Get(string name)
    {
        var user = await _userManager.FindByNameAsync(name);
        if (user == null)
            return ApiResponse<TokenResponseDto>.Fail($"User not found with name: {name}",
                StatusCodes.Status404NotFound);

        var tokenDto = _tokenService.CreateToken(user);
        return ApiResponse<TokenResponseDto>.Success(tokenDto, StatusCodes.Status200OK);
    }
}