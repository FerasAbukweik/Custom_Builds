using System.Net;
using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.DTO.Account;
using Custom_Builds.Core.Enums;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class AccountService(
    UserManager<ApplicationUser> userManager,
    ICookieService cookieService,
    ITokensService tokensService,
    ILogger<AccountService> logger,
    IChatGroupService chatGroupService,
    IUsersRepository usersRepository) : IAccountService
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
    public async Task<Result> RegisterAsync(RegisterDTO registerInfo, CancellationToken cancellationToken = default)
    {
        // check if email already exists
        var doesUserExistResult = await DoesUserExist(registerInfo, cancellationToken);
        if (doesUserExistResult.IsSuccess)
            return Result.Failure(doesUserExistResult.Value!);

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
            string errors = string.Join(" | ", addUserResult.Errors.Select(e => e.Description));
            logger.LogError("{serviceName}.{methodName} failed to create user\nErrors: {errors}",
                nameof(AccountService), nameof(RegisterAsync), errors);
            return Result.Failure(errors);
        }

        // add user to his role
        var addToRoleResult = await userManager.AddToRoleAsync(newUser, nameof(RolesEnum.User));
        if (!addToRoleResult.Succeeded)
        {
            string errors = string.Join(" | ", addToRoleResult.Errors.Select(e => e.Description));
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
    
    
    
    private async Task<Result<string>> DoesUserExist(RegisterDTO toUserCreate, CancellationToken cancellationToken = default)
    {
        // check if user already Exists
        var existingUsers = await usersRepository.FilterAsync((u =>
                (u.UserName!.ToLower() == toUserCreate.UserName.ToLower() || 
                 u.Email!.ToLower() == toUserCreate.Email.ToLower() || 
                 u.PhoneNumber == toUserCreate.PhoneNumber)
            ),cancellationToken: cancellationToken);

        // if user already exist generate error message and return failure
        if (existingUsers.Any())
        {
            bool isEmailUsed = false , isPhoneUsed = false , isUserNameUsed = false;

            // see what is used
            foreach (var user in existingUsers)
            {
                if (user.UserName == toUserCreate.UserName) isUserNameUsed = true;
                if (user.Email == toUserCreate.Email) isEmailUsed = true;
                if (user.PhoneNumber == toUserCreate.PhoneNumber) isPhoneUsed = true;
                
                if(isEmailUsed && isPhoneUsed && isUserNameUsed) break;
            }
 
            // collect used fields in list
            var usedFields = new List<string>();
            if (isEmailUsed) usedFields.Add("Email");
            if (isPhoneUsed) usedFields.Add("Phone number");
            if (isUserNameUsed) usedFields.Add("Username");

            // generate error message
            string fieldsText = string.Join(",\n", usedFields);
            string verb = usedFields.Count == 1 ? "\nis already used." : "\nare already used.";
            string errorMessage = $"{fieldsText} {verb}";

            return Result<string>.Success(errorMessage);
        }

        return Result<string>.Failure("");
    }
}