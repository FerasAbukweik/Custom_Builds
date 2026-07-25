using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Auth;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers;

public class AuthController(
    IAuthService authService,
    ICookieService cookieService) : ApplicationControllerBase
{
    // check token
    [HttpPost("[action]")]
    [Authorize]
    public IActionResult IsAuthenticated()
    {
        return Ok();
    }
    
    // login
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody]LoginDTO loginInfo)
    {
        Result result = await authService.LoginAsync(loginInfo);

        return result.ToActionResult();
    }
    
    // logout
    [HttpPost("[action]")]
    [Authorize]
    public ActionResult Logout(CancellationToken cancellationToken = default)
    {
        // get currUser id --only for logging
        var getCurrUserId = User.GetId();
        if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
        authService.Logout(getCurrUserId.Value!);

        return Ok();
    }
    
    // update tokens
    [HttpPost("[action]")]
    public async Task<IActionResult> UpdateTokens(CancellationToken cancellationToken = default)
    {
        var getRefreshTokenResult = cookieService.GetRefreshToken();
        if (!getRefreshTokenResult.IsSuccess) return ((Result)getRefreshTokenResult).ToActionResult();
        
        Result result = await authService.UpdateTokensAsync(getRefreshTokenResult.Value!, cancellationToken);

        return result.ToActionResult();
    }
}