using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.ChatGroup;
using Custom_Builds.Core.DTO.Lazy;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IChatGroupService
{
    Task<Result<ChatGroupDTO>> AddChatGroupAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<Guid>> GetChatGroupIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<ChatGroupDTO>>> LazyGetChatGroupsAsync(LazyDTO lazyData, CancellationToken cancellationToken = default);
}