using Custom_Builds.Core.DTO;
using Custom_Builds.Core.DTO.Message;

namespace custom_Peripherals.IHub
{
    public interface IChatHub
    {
        Task ReceiveMessageAsync(MessageDTO message);
        Task UserIsTypingAsync(Guid chatGroupId);
        Task UserStoppedTypingAsync(Guid chatGroupId);
    }
}
