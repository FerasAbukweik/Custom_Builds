using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;

namespace custom_Peripherals.IHub
{
    public interface IChatHub
    {
        Task<Result> ReceiveMessageAsync(MessageDTO message);
        Task UserIsTypingAsync(Guid senderId);
        Task UserStoppedTypingAsync(Guid senderId);
    }
}
