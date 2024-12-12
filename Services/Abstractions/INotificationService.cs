using PersonalAccount.API.Models.Entities.Agiles.Comments;

namespace PersonalAccount.API.Services.Abstractions
{
    public interface INotificationService
    {
        Task SendMentionNotificationsAsync(Comment comment, List<Guid> mentionedUserIds);
        Task SendReactionNotificationAsync(Guid commentId, Guid userId, string emoji, DateTime reactionTime);
    }
}
