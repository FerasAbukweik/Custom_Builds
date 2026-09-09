using Custom_Builds.Core.Common;
using Custom_Builds.Core.Domain.Entities;
using Custom_Builds.Core.DTO.ChatGroup;
using Custom_Builds.Core.DTO.Lazy;
using Custom_Builds.Core.DTO.Message;
using Custom_Builds.Core.Interfaces.RepositoryContracts;
using Custom_Builds.Core.Interfaces.ServiceContracts;

namespace Custom_Builds.Infrastructure.Services;

public class ChatGroupService(
    IChatGroupRepository chatGroupRepository,
    IMessageService messageService) : IChatGroupService
{
    public async Task<Result<ChatGroupDTO>> AddChatGroupAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        ChatGroup newChatGroup = new ChatGroup()
        {
            UserId = userId
        };

        // add to DB
        chatGroupRepository.Add(newChatGroup);

        // add Welcome message + save changes
        await messageService.AddAsync(new MessageAddDTO()
        {
            Content = "Welcome! Feel free to ask any questions or share your thoughts here. We're here to help you with anything you need. Enjoy your chat experience!",
            ChatGroupId = newChatGroup.Id
        },userId, cancellationToken);
        
        return Result<ChatGroupDTO>.Success(newChatGroup.toDTO());
    }
    public async Task<Result<Guid>> GetChatGroupIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var chatGroupId = await chatGroupRepository.GetUserChatGroupIdAsync(userId, cancellationToken);
        if (chatGroupId == null)
            return Result<Guid>.Failure("User doesnt have chat group");

        return Result<Guid>.Success(chatGroupId.Value);
    }
    public async Task<Result<IReadOnlyList<ChatGroupDTO>>> LazyGetChatGroupsAsync(LazyDTO lazyData, CancellationToken cancellationToken = default)
    {
        var result = await chatGroupRepository.FilterAsync(
            cg => true,
            [cg => cg.User, cg => cg.Messages],
            cg => cg.Messages.Max(m => m.CreatedAt),
            true,
            lazyData.Taken,
            lazyData.SectionSize,
            cancellationToken);

        return Result<IReadOnlyList<ChatGroupDTO>>.Success(result.Select(r => r.toDTO()).ToList());
    }
}