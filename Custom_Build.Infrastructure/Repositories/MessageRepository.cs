using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.Domain.RepositoryContracts;
using Custom_Builds.Core.DTO;
using Custom_Builds.Core.Models;
using Custom_Builds.Core.ServiceContracts.ICurrUserServices;
using Custom_Builds.Infrastructure.DBcontext;

namespace Custom_Builds.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IGetCurrUserService _currUserServices;

        public MessageRepository(ApplicationDbContext dbContext,
                                 IGetCurrUserService currUserServices)
        {
            _dbContext = dbContext;
            _currUserServices = currUserServices;
        }
        public async Task<Result<Message>> Add(SentMessageDTO message)
        {
            var getCurrUserIdResult = _currUserServices.GetUserId();
            if (!getCurrUserIdResult.IsSuccess) return getCurrUserIdResult.MapFailure<Message>();

            var newMessage = new Message
            {
                Id = Guid.NewGuid(),
                ReceiverId = message.ReceiverId,
                Content = message.Content,
                MessageType = message.MessageType,
                FileName = message.FileName,
                SenderId = getCurrUserIdResult.Value!
            };

            _dbContext.Messages.Add(newMessage);
            await _dbContext.SaveChangesAsync();

            return Result<Message>.Success(newMessage);
        }
    }
}
