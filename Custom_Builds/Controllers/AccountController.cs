using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Infrastructure.DBcontext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace custom_Peripherals.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRegisterAccountService _registerAccountService;
        private readonly ILoginAccountService _loginAccountService;
        private readonly IDeleteUserService _deleteCurrentUserService;
        private readonly ILogoutAccountService _logoutAccountService;
        private readonly IGetCurrUserService _getCurrUserService;

        public AccountController(
                IRegisterAccountService registerAccountService,
                ILoginAccountService loginAccountService,
                IDeleteUserService deleteCurrentUserService,
                ILogoutAccountService logoutAccountService,
                IGetCurrUserService currUserServices)
        {
            _registerAccountService = registerAccountService;
            _loginAccountService = loginAccountService;
            _deleteCurrentUserService = deleteCurrentUserService;
            _logoutAccountService = logoutAccountService;
            _getCurrUserService = currUserServices;
        }


        // register
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody]RegisterDTO registerInfo)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();
                return BadRequest(errors);
            }

            Result result = await _registerAccountService.RegisterAsync(registerInfo);

            return result.ToActionResult();
        }

        // login
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody]LoginDTO loginInfo)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();
                return BadRequest(errors);
            }

            Result result = await _loginAccountService.LoginAsync(loginInfo);

            return result.ToActionResult();
        }


        // delete user
        [HttpDelete("[action]/{toDelUserID}")]
        public async Task<IActionResult> DeleteUser([FromRoute]Guid? toDelUserID)
        {
            
            var getTargetUserIdResult = _getCurrUserService.GetTargetUserId(toDelUserID);
            if (!getTargetUserIdResult.IsSuccess)
            {
                // convert to Result so its guranteed we return ActionResult and not ActionResult<Data>
                return ((Result)getTargetUserIdResult).ToActionResult();
            }

            // delete the user
            Result result = await _deleteCurrentUserService.DeleteUserAsync(getTargetUserIdResult!.Value);

            return result.ToActionResult();
        }

        // logout
        [HttpDelete("[action]")]
        public async Task<IActionResult> Logout()
        {
            Result result = await _logoutAccountService.LogoutAsync();

            return result.ToActionResult();
        }
    }
}
