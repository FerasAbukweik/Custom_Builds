using Custom_Builds.Core.Domain.Identity;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrTokenService;
using Custom_Builds.Core.ServiceContracts.IMessageServices;
using Microsoft.AspNetCore.Identity;

namespace Custom_Builds.Core.Services.MessageServices
{
    public class GetMessageService : IGetMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IGetCurrUserService _getCurrUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public GetMessageService(IMessageRepository messageRepository,
                                 IGetCurrUserService getCurrUserService,
                                 UserManager<ApplicationUser> userManager)
        {
            _messageRepository = messageRepository;
            _getCurrUserService = getCurrUserService;
            _userManager = userManager;
        }

        public async Task<Result<List<MessageDTO>>> GetMessagesAsync(LazyDTO lazyLoadData)
        {
            var getCurrUserIdResult = _getCurrUserService.GetUserId();
            if (!getCurrUserIdResult.IsSuccess) return getCurrUserIdResult.MapFailure<List<MessageDTO>>();

            var getMessagesResult = await _messageRepository.GetMessagesAsync(new LazyGetUserDataDTO {
                UserId = getCurrUserIdResult.Value!,
                ElementsPerSection = lazyLoadData.ElementsPerSection,
                Taken = lazyLoadData.Taken
            });
            if (!getMessagesResult.IsSuccess) return getMessagesResult.MapFailure<List<MessageDTO>>();


            var messages = new List<MessageDTO>();
            foreach (var m in getMessagesResult.Value!)
            {
                if (m.Sender != null)
                {
                    var roles = await _userManager.GetRolesAsync(m.Sender);
                    messages.Add(m.toDTO(m.Sender.UserName ?? "unknown", roles.FirstOrDefault() ?? "unknown", getCurrUserIdResult.Value!));
                }
                else
                {   
                    messages.Add(m.toDTO("unknown", "unknown", getCurrUserIdResult.Value!));
                }
            }


            return Result<List<MessageDTO>>.Success(messages);
        }
    }
}
