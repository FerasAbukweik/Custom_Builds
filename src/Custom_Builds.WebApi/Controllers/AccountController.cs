using System.Security.Claims;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Account;
using Custom_Builds.Core.DTO.Auth;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using custom_Peripherals.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace custom_Peripherals.Controllers
{
    public class AccountController(
        IAccountService accountService
        ) : ApplicationControllerBase
    {
        // register
        [HttpPost("[action]")]
        [Transactional]
        public async Task<ActionResult<UserDTO>> Register([FromBody]RegisterDTO registerInfo)
        {
            var result = await accountService.RegisterAsync(registerInfo);

            return result.ToActionResult();
        }

        // delete user
        [HttpDelete("[action]/{toDelUserID}")]
        [Transactional]
        [Authorize]
        public async Task<IActionResult> DeleteUser()
        {
            // get currUser id
            var getCurrUserId = User.GetId();
            if (!getCurrUserId.IsSuccess) return ((Result)getCurrUserId).ToActionResult();
            
            // delete the user
            Result result = await accountService.DeleteUserAsync(getCurrUserId.Value!);

            return result.ToActionResult();
        }
    }
}
