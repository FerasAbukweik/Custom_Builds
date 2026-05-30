using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IChatGroupServices
{
    public interface IGetChatGroupService
    {
        Task<Result<Guid>> GetChatGroupIdAsync(Guid userId);
    }
}
