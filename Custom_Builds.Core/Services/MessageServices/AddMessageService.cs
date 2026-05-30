using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.IMessageServices;
using Microsoft.AspNetCore.Identity;

namespace Custom_Builds.Core.Services.MessageServices
{
    public class AddMessageService : IAddMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IGetCurrUserService _getCurrUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AddMessageService(IMessageRepository messageRepository,
                                 IGetCurrUserService getCurrUserService,
                                 UserManager<ApplicationUser> userManager)
        {
            _messageRepository = messageRepository;
            _getCurrUserService = getCurrUserService;
            _userManager = userManager;
        }

        public async Task<Result<MessageDTO>> AddAsync(AddMessageDTO toAdd , Guid ChatGroupId)
        {
            // get curr logged in user id
            var getCurrUserId = _getCurrUserService.GetUserId();
            if (!getCurrUserId.IsSuccess) return getCurrUserId.MapFailure<MessageDTO>();

            // new message
            Message newMessage = new Message()
            {
                Id = Guid.NewGuid(),
                Content = toAdd.Content,
                CreatedAt = DateTime.UtcNow,
                FileName = toAdd.FileName,
                MessageType = toAdd.MessageType,
                SenderId = getCurrUserId.Value!,
                ChatGroupId = ChatGroupId
            };


            // add message to DB
            var result = await _messageRepository.Add(newMessage);
            if (!result.IsSuccess) return result.MapFailure<MessageDTO>();

            // get info about curr user to add it to dto
            var CurrUser = await _userManager.FindByIdAsync(getCurrUserId.Value!.ToString());

            if (CurrUser == null)
            {
                return Result<MessageDTO>.Failure("curr user not found");
            }

            var role = await _userManager.GetRolesAsync(CurrUser);
            if (role == null || !role.Any())
            {
                return Result<MessageDTO>.Failure("User role not found");
            }

            return Result<MessageDTO>.Success(result.Value!.toDTO(CurrUser.UserName ?? "unknown" , role.First(), getCurrUserId.Value!));
        }
    }
}
