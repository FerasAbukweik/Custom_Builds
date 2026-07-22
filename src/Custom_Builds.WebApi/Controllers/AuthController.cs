using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Auth;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers;

public class AuthController(IAccountService accountService) : ApplicationControllerBase
{
    // check token
    [HttpGet("[action]")]
    [Authorize]
    public IActionResult IsAuthenticated()
    {
        return Ok();
    }
    
    // login
    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody]LoginDTO loginInfo)
    {
        Result result = await accountService.LoginAsync(loginInfo);

        return result.ToActionResult();
    }
    
    // logout
    [HttpDelete("[action]")]
    [Authorize]
    public ActionResult Logout(CancellationToken cancellationToken = default)
    {
        // get currUser id --only for logging
        var getCurrUserId = User.GetId();
        if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
        accountService.Logout(getCurrUserId.Value!);

        return Ok();
    }
}