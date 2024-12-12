using Global.Infrastructure.Exceptions.PersonalAccount;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Entities.Agiles.Comments;
using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class NotificationService : INotificationService
{
    private readonly AgileDbContext _dbContext;

    public NotificationService(AgileDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SendMentionNotificationsAsync(Comment comment, List<Guid> mentionedUserIds)
    {
        try
        {
            foreach (var userId in mentionedUserIds)
            {
                var user = await _dbContext.Users.FindAsync(userId);
                if (user == null)
                    throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"User with ID {userId} not found.");

                var notification = new Notification
                {
                    NotificationText = $"You were mentioned in a comment: {comment.Text}",
                    CommentText = comment.Text,
                    SentAt = DateTime.UtcNow,
                    SenderId = comment.UserId,
                    Sender = comment.User,
                    Recipients = new List<User> { user }
                };

                _dbContext.Notifications.Add(notification);
            }

            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task SendReactionNotificationAsync(Guid commentId, Guid userId, string emoji, DateTime reactionTime)
    {
        try
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
                throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"User with ID {userId} not found.");

            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment == null)
                throw new PersonalAccountException(PersonalAccountErrorType.CommentNotFound, $"Comment with ID {commentId} not found.");

            var notification = new Notification
            {
                NotificationText = $"Your comment received a reaction: {emoji}",
                CommentText = comment.Text,
                SentAt = reactionTime,
                SenderId = userId,
                Sender = user,
                Recipients = new List<User> { comment.User }
            };

            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }
}
