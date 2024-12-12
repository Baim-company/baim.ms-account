using PersonalAccount.API.Models.Dtos.Agiles.Comments;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.Comments;

namespace PersonalAccount.API.Services.Interfaces;

public interface ICommentService
{
    Task<SimpleApiResponse> AddCommentAsync(CreateCommentModel model);
    Task<Response<List<Comment>>> GetCommentsByTicketIdAsync(Guid ticketId);
    Task<Response<List<Comment>>> GetCommentsBySubTicketIdAsync(Guid subTicketId);
    Task<SimpleApiResponse> UpdateCommentAsync(UpdateCommentModel model);
    Task<SimpleApiResponse> AddReactionAsync(Guid commentId, AddReactionModel model);
    Task<SimpleApiResponse> RemoveReactionAsync(Guid commentId, Guid reactionId);
    Task<SimpleApiResponse> UpdateReactionAsync(Guid reactionId, string newEmoji);
    Task<Response<string>> DeleteCommentByIdAsync(Guid commentId);
}