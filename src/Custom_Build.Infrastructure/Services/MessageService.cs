using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Message;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class MessageService(
    IMessageRepository messageRepository,
    ILogger<MessageService> logger) : IMessageService
{
    public async Task<Result<MessageDTO>> AddAsync(
        MessageAddDTO toMessageAdd,
        Guid senderId,
        CancellationToken cancellationToken  = default)
    {
        // new message
        Message newMessage = new Message()
        {
            Content = toMessageAdd.Content,
            CreatedAt = DateTime.UtcNow,
            SenderId = senderId,
            ChatGroupId = toMessageAdd.ChatGroupId
        };

        // add message to DB
        messageRepository.Add(newMessage);

        if (!await messageRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(MessageService), nameof(AddAsync));
            return Result<MessageDTO>.Failure("failed saving changes to DB");
        }

        var message = await messageRepository.GetByIdAsync(newMessage.Id,[m => m.Sender], cancellationToken);
        if(message == null)
            return Result<MessageDTO>.Failure("message was not added");


        return Result<MessageDTO>.Success(message.toDTO(senderId));
    }
    public async Task<Result<IReadOnlyList<MessageDTO>>> GetMessagesAsync(
        LazyDTO lazyData,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        // get messages from repository
        var messages = await messageRepository.LazyGetMessagesAsync(lazyData, userId, cancellationToken);
        
        // map messages to DTO
        return Result<IReadOnlyList<MessageDTO>>.Success(messages.Select(m => m.toDTO(userId)).ToList());
    }
}