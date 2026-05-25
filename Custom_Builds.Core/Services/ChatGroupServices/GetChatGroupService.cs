using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.IChatGroupServices;
using System.Net;

namespace Custom_Builds.Core.Services.ChatGroupServices
{
    public class GetChatGroupService : IGetChatGroupService
    {
        private readonly IChatGroupRepository _chatGroupRepository;
        private readonly IAddChatGroupService _addChatGroupService;

        public GetChatGroupService(IChatGroupRepository chatGroupRepository,
                                   IAddChatGroupService addChatGroupService)
        {
            _chatGroupRepository = chatGroupRepository;
            _addChatGroupService = addChatGroupService;
        }

        public async Task<Result<Guid>> GetChatGroupId(Guid userId)
        {
            var getIdResult = await _chatGroupRepository.GetUserChatGroupIdAsync(userId);

            if (!getIdResult.IsSuccess)
            {
                if(getIdResult.StatusCode == HttpStatusCode.NotFound)
                {
                    var addResult = await _addChatGroupService.AddChatGroupAsync(userId);
                    if(!addResult.IsSuccess) return addResult.MapFailure<Guid>();

                    return Result<Guid>.Success(addResult.Value!.Id);
                }

                return getIdResult;
            }

            return getIdResult;
        }
    }
}
