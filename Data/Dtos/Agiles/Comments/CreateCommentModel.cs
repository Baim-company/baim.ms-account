namespace PersonalAccount.API.Models.Dtos.Agiles.Comments;

public class CreateCommentModel
{
    public string Text { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectTaskId { get; set; }
    public List<CommentFileModel> Files { get; set; } = new List<CommentFileModel>();
}