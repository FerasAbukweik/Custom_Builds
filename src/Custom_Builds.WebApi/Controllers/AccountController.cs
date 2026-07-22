using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Account;
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
        public async Task<IActionResult> Register([FromBody]RegisterDTO registerInfo)
        {
            Result result = await accountService.RegisterAsync(registerInfo);

            return result.ToActionResult();
        }

        // delete user
        [HttpDelete("[action]/{toDelUserID}")]
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
