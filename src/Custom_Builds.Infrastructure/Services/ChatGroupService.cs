using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.ChatGroup;
using Custom_Builds.Core.DTO.Message;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;
using Microsoft.Extensions.Logging;

namespace Custom_Builds.Infrastructure.Services;

public class ChatGroupService(
    IChatGroupRepository chatGroupRepository,
    IMessageService messageService,
    ILogger<ChatGroupService> logger) : IChatGroupService
{
    public async Task<Result<ChatGroupDTO>> AddChatGroupAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        ChatGroup toAdd = new ChatGroup()
        {
            UserId = userId
        };

        // add to DB
        chatGroupRepository.Add(toAdd);

        if (!await chatGroupRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(ChatGroupService),  nameof(AddChatGroupAsync));
            return Result<ChatGroupDTO>.Failure("failed saving changes to DB");
        }

        // add Welcome message
        var messageResult = await messageService.AddAsync(new MessageAddDTO()
        {
            Content = "Welcome to your new chat group! Feel free to ask any questions or share your thoughts here. We're here to help you with anything you need. Enjoy your chat experience!",
            ChatGroupId = toAdd.Id
        },userId, cancellationToken);
        
        if (!await chatGroupRepository.SaveChangesAsync(cancellationToken))
        {
            logger.LogError("{serviceName}.{methodName} failed saving changes to DB",
                nameof(ChatGroupService),  nameof(AddChatGroupAsync));
            return Result<ChatGroupDTO>.Failure("failed saving changes to DB");
        }

        return Result<ChatGroupDTO>.Success(toAdd.toDTO());
    }
    public async Task<Result<Guid>> GetChatGroupIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var chatGroupId = await chatGroupRepository.GetUserChatGroupIdAsync(userId, cancellationToken);
        if (chatGroupId == null)
            return Result<Guid>.Failure("User doesnt have chat group");

        return Result<Guid>.Success(chatGroupId.Value);
    }
}