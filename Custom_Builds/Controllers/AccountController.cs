using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositryContracts;
using Custom_Builds.Core.Domain.TokenEntities;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.extensionMethods;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IAccountServices;
using Custom_Builds.Core.Utils;
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
        private readonly IDeleteCurrentUserService _deleteCurrentUserService;
        private readonly ILogoutAccountService _logoutAccountService;

        public AccountController(
                IRegisterAccountService registerAccountService,
                ILoginAccountService loginAccountService,
                IDeleteCurrentUserService deleteCurrentUserService,
                ILogoutAccountService logoutAccountService)
        {
            _registerAccountService = registerAccountService;
            _loginAccountService = loginAccountService;
            _deleteCurrentUserService = deleteCurrentUserService;
            _logoutAccountService = logoutAccountService;
        }


        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterDTO registerInfo)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();
                return BadRequest(errors);
            }

            Result result = await _registerAccountService.RegisterAsync(Response, registerInfo);

            return result.ToActionResult();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginDTO loginInfo)
        {
            if (!ModelState.IsValid)
            {
                string errors = ModelState.CollectErrors();
                return BadRequest(errors);
            }

            Result result = await _loginAccountService.LoginAsync(Response, loginInfo);

            return result.ToActionResult();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCurrentUser()
        {
            Result result = await _deleteCurrentUserService.DeleteUserAsync(Response, User);

            return result.ToActionResult();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Logout()
        {
            Result result = await _logoutAccountService.LogoutAsync();

            return result.ToActionResult();
        }
    }
}
