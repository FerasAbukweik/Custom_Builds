using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IMessageServices
{
    public interface IEditMessageService
    {
        Task<Result> SetUserMessagesToNull(Guid userId);
    }
}
