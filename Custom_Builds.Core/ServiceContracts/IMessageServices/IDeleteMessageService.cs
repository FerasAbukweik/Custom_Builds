using Custom_Builds.Core.Models;

namespace Custom_Builds.Core.ServiceContracts.IMessageServices
{
    public interface IDeleteMessageService
    {
        Task<Result> SetUserMessagesToNull();
    }
}
