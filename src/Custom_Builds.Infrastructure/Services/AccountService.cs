using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO.Account;
using Custom_Builds.Core.DTO.Auth;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class AccountService(
    UserManager<ApplicationUser> userManager,
    ICookieService cookieService,
    ITokensService tokensService,
    ILogger<AccountService> logger,
    IChatGroupService chatGroupService) : IAccountService
{
    public async Task<Result<ApplicationUser>> DeleteUserAsync(Guid userId)
    {
        // get user object so we can delete it using _userManager.DeleteAsync(user) and check if its an admin
        ApplicationUser? userToDel = await userManager.FindByIdAsync(userId.ToString());
        if (userToDel == null)
        {
            logger.LogWarning("{serviceName}.{methodName} failed removing user with id {userId} because user want found",
                nameof(AccountService), nameof(DeleteUserAsync), userId);
            return Result<ApplicationUser>.Failure("User was not found", HttpStatusCode.NotFound);
        }

        // remove User from Users table
        // also removes all user refreshTokens because DeleteBehavior.cascade
        var result = await userManager.DeleteAsync(userToDel);
        if (!result.Succeeded)
        {
            string errors = string.Join(" | ", result.Errors);
            logger.LogWarning("{serviceName}.{methodName} failed removing user with id {userId} because\nErrors: {errors}",
                nameof(AccountService), nameof(DeleteUserAsync), userId, errors);
            return Result<ApplicationUser>.Failure(errors);
        }

        logger.LogInformation("{serviceName}.{methodName} user with id: {userId} was removed",
            nameof(AccountService), nameof(DeleteUserAsync), userId);
        return Result<ApplicationUser>.Success(userToDel);
    }
    public async Task<Result> LoginAsync(LoginDTO loginInfo) 
    {
        // find user by email
        ApplicationUser? user = await userManager.FindByEmailAsync(loginInfo.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, loginInfo.Password))
        {
            logger.LogWarning("{serviceName}.{methodName} failed login attempt for email: {email}",
                nameof(AccountService), nameof(LoginAsync), loginInfo.Email);
            return Result.Failure("Wrong Email or Password" , HttpStatusCode.Unauthorized);
        }

        // generate Tokens
        var generateTokensResult = await tokensService.GenerateTokens(user);
        if (!generateTokensResult.IsSuccess) return generateTokensResult;
        
        // store tokens in cookies response
        var storeTokensResult = cookieService.SetTokens(generateTokensResult.Value!);
        if (!storeTokensResult.IsSuccess) return storeTokensResult;

        logger.LogInformation("{serviceName}.{methodName} user with id: {userId} logged in",
            nameof(AccountService), nameof(LoginAsync), user.Id);
        
        return Result.Success();
    }
    public void Logout(Guid userId)
    {
        cookieService.RemoveTokens();
        
        logger.LogWarning("{serviceName}.{methodName} user with id: {userId} logged out",
            nameof(AccountService), nameof(Logout), userId);
    }
    public async Task<Result> RegisterAsync(RegisterDTO registerInfo, CancellationToken cancellationToken = default)
    {
        // check if email already exists
        if (await userManager.FindByEmailAsync(registerInfo.Email) != null)
        {
            return Result.Failure("Email already used");
        }

        // new user to add
        ApplicationUser newUser = new ApplicationUser()
        {
            UserName = registerInfo.UserName,
            Email = registerInfo.Email,
            PhoneNumber = registerInfo.PhoneNumber,
        };

        // add user to identityUser table
        var addUserResult = await userManager.CreateAsync(newUser, registerInfo.Password);
        if (!addUserResult.Succeeded)
        {
            string errors = string.Join(" | ", addUserResult.Errors);
            logger.LogError("{serviceName}.{methodName} failed to create user\nErrors: {errors}",
                nameof(AccountService), nameof(RegisterAsync), errors);
            return Result.Failure(errors);
        }

        // add user to his role
        var addToRoleResult = await userManager.AddToRoleAsync(newUser, registerInfo.role.ToString());
        if (!addToRoleResult.Succeeded)
        {
            string errors = string.Join(" | ", addToRoleResult.Errors);
            logger.LogError("{serviceName}.{methodName} failed adding user to role\nErrors: {errors}",
                nameof(AccountService), nameof(RegisterAsync), errors);
            return Result.Failure(errors);
        }
        
        // add chatGroup for the user
        var addGroupResult = await chatGroupService.AddChatGroupAsync(newUser.Id, cancellationToken);
        if (!addGroupResult.IsSuccess) return addGroupResult;

        
        // generate Tokens
        var generateTokensResult = await tokensService.GenerateTokens(newUser, cancellationToken);
        if (!generateTokensResult.IsSuccess) return generateTokensResult;
        
        // store tokens in response cookies
        var storeTokensResult = cookieService.SetTokens(generateTokensResult.Value!);
        if(!storeTokensResult.IsSuccess) return  storeTokensResult;

        return Result.Success();
    }
}