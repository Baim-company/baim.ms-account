using PersonalAccount.API.Models.Dtos.Agiles.Comments;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.Comments;
using PersonalAccount.API.Services.Abstractions;
using Microsoft.EntityFrameworkCore; 
using PersonalAccount.API.Services.Interfaces;
using PersonalAccount.API.Data.DbContexts;
using Global.Infrastructure.Exceptions.PersonalAccount;

namespace PersonalAccount.API.Services.Implementations;

public class CommentService : ICommentService
{
    private readonly AgileDbContext _baimDbContext;
    private readonly INotificationService _notificationService;

    public CommentService(AgileDbContext dbContext, INotificationService notificationService)
    {
        _baimDbContext = dbContext;
        _notificationService = notificationService;
    }

    public async Task<SimpleApiResponse> AddCommentAsync(CreateCommentModel model)
    {
        try
        {
            var comment = new Comment
            {
                Text = model.Text,
                CreatedAt = DateTime.UtcNow,
                UserId = model.UserId,
                ProjectTaskId = model.ProjectTaskId,
                Files = new List<CommentFile>()
            };

            if (model.Files != null)
            {
                foreach (var file in model.Files)
                {
                    var commentFile = new CommentFile
                    {
                        Id = Guid.NewGuid(),
                        FileName = file.FileName,
                        FileExtension = file.FileExtension,
                        FileData = Convert.FromBase64String(file.FileContentBase64),
                        Comment = comment
                    };

                    comment.Files.Add(commentFile);
                }
            }

            var mentionedUserIds = ExtractMentionedUsers(model.Text);
            foreach (var userId in mentionedUserIds)
            {
                comment.Mentions.Add(new Mention
                {
                    UserId = userId,
                    CommentId = comment.Id
                });
            }

            _baimDbContext.Comments.Add(comment);
            await _baimDbContext.SaveChangesAsync();

            await _notificationService.SendMentionNotificationsAsync(comment, mentionedUserIds);

            return new SimpleApiResponse("Comment added successfully");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<List<Comment>>> GetCommentsByTicketIdAsync(Guid ticketId)
    {
        try
        {
            var projectTask = await _baimDbContext.ProjectTasks
                .Where(pt => pt.ProjectTicketId == ticketId)
                .Include(pt => pt.Comments)
                .ThenInclude(c => c.User)
                .Include(pt => pt.Comments)
                .ThenInclude(c => c.Files)
                .Include(pt => pt.Comments)
                .ThenInclude(c => c.Reactions)
                .FirstOrDefaultAsync();

            if (projectTask == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "No project task found for the given ticket ID.");

            var comments = projectTask.Comments.ToList();

            return new Response<List<Comment>>("Comments retrieved successfully", comments);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<List<Comment>>> GetCommentsBySubTicketIdAsync(Guid subTicketId)
    {
        try
        {
            var projectTask = await _baimDbContext.ProjectTasks
                .Where(pt => pt.ProjectSubTicketId == subTicketId)
                .Include(pt => pt.Comments)
                .ThenInclude(c => c.User)
                .Include(pt => pt.Comments)
                .ThenInclude(c => c.Files)
                .Include(pt => pt.Comments)
                .ThenInclude(c => c.Reactions)
                .FirstOrDefaultAsync();

            if (projectTask == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "No project task found for the given sub-ticket ID.");

            var comments = projectTask.Comments.ToList();

            return new Response<List<Comment>>("Comments retrieved successfully", comments);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<Response<string>> DeleteCommentByIdAsync(Guid commentId)
    {
        try
        {
            var comment = await _baimDbContext.Comments.FindAsync(commentId);

            if (comment == null)
                throw new PersonalAccountException(PersonalAccountErrorType.CommentNotFound, "Comment not found.");

            _baimDbContext.Comments.Remove(comment);
            await _baimDbContext.SaveChangesAsync();

            return new Response<string>("Comment deleted successfully", null);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<SimpleApiResponse> UpdateCommentAsync(UpdateCommentModel model)
    {
        try
        {
            var existingComment = await _baimDbContext.Comments
                .AsNoTracking()
                .Include(c => c.Files)
                .FirstOrDefaultAsync(c => c.Id == model.CommentId);

            if (existingComment == null)
                throw new PersonalAccountException(PersonalAccountErrorType.CommentNotFound, "Comment not found.");

            var projectTaskExists = await _baimDbContext.ProjectTasks
                .AnyAsync(pt => pt.Id == model.ProjectTaskId);

            if (!projectTaskExists)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "Project task not found.");

            var commentToUpdate = await _baimDbContext.Comments
                .Include(c => c.Files)
                .FirstOrDefaultAsync(c => c.Id == model.CommentId);

            commentToUpdate.Text = model.Text;
            commentToUpdate.ProjectTaskId = model.ProjectTaskId;

            var existingFiles = commentToUpdate.Files.ToList();
            _baimDbContext.CommentFiles.RemoveRange(existingFiles);

            if (model.Files != null)
            {
                foreach (var file in model.Files)
                {
                    var newFile = new CommentFile
                    {
                        Id = Guid.NewGuid(),
                        FileName = file.FileName,
                        FileExtension = file.FileExtension,
                        FileData = Convert.FromBase64String(file.FileContentBase64),
                        CommentId = model.CommentId
                    };

                    _baimDbContext.CommentFiles.Add(newFile);
                }
            }

            _baimDbContext.Comments.Update(commentToUpdate);

            await _baimDbContext.SaveChangesAsync();

            return new SimpleApiResponse("Comment updated successfully");
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new PersonalAccountException(PersonalAccountErrorType.ConcurrencyError, "Concurrency error occurred. Please try again.");
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<SimpleApiResponse> AddReactionAsync(Guid commentId, AddReactionModel model)
    {
        try
        {
            var commentExists = await _baimDbContext.Comments
                .AnyAsync(c => c.Id == commentId);

            if (!commentExists)
                throw new PersonalAccountException(PersonalAccountErrorType.CommentNotFound, "Comment not found.");

            var reaction = new Reaction
            {
                Id = Guid.NewGuid(),
                Emoji = model.Emoji,
                UserId = model.UserId,
                CommentId = commentId,
                CreatedAt = DateTime.UtcNow
            };

            _baimDbContext.Reactions.Add(reaction);

            await _baimDbContext.SaveChangesAsync();

            await _notificationService.SendReactionNotificationAsync(commentId, model.UserId, model.Emoji, DateTime.UtcNow);

            return new SimpleApiResponse("Reaction added successfully");
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new PersonalAccountException(PersonalAccountErrorType.ConcurrencyError, "Concurrency error occurred. Please try again.");
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<SimpleApiResponse> RemoveReactionAsync(Guid commentId, Guid reactionId)
    {
        try
        {
            var reaction = await _baimDbContext.Reactions.FindAsync(reactionId);
            if (reaction == null || reaction.CommentId != commentId)
                throw new PersonalAccountException(PersonalAccountErrorType.ReactionNotFound, "Reaction not found or does not belong to this comment.");

            _baimDbContext.Reactions.Remove(reaction);
            await _baimDbContext.SaveChangesAsync();

            return new SimpleApiResponse("Reaction removed successfully");
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<SimpleApiResponse> UpdateReactionAsync(Guid reactionId, string newEmoji)
    {
        try
        {
            var reaction = await _baimDbContext.Reactions
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == reactionId);

            if (reaction == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ReactionNotFound, "Reaction not found.");

            var updatedReaction = new Reaction
            {
                Id = reaction.Id,
                Emoji = newEmoji,
                CreatedAt = reaction.CreatedAt,
                UserId = reaction.UserId,
                CommentId = reaction.CommentId
            };

            _baimDbContext.Reactions.Update(updatedReaction);

            await _baimDbContext.SaveChangesAsync();

            return new SimpleApiResponse("Reaction updated successfully");
        }
        catch (Exception)
        {
            throw;
        }
    }


    private List<Guid> ExtractMentionedUsers(string text)
    {
        var mentionedUserIds = new List<Guid>();
        return mentionedUserIds.Distinct().ToList();
    }
}