using Custom_Builds.Core.Common;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Message;

namespace Custom_Builds.Core.Interfaces.ServiceContracts;

public interface IMessageService
{
    Task<Result<MessageDTO>> AddAsync(
        MessageAddDTO toMessageAdd,
        Guid senderId,
        CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<MessageDTO>>> GetMessagesAsync(
        LazyDTO lazyData,
        Guid userId,
        CancellationToken cancellationToken = default);
}