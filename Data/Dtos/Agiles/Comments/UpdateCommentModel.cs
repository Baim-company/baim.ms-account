namespace PersonalAccount.API.Models.Dtos.Agiles.Comments;

public class UpdateCommentModel
{
    public Guid CommentId { get; set; }
    public string Text { get; set; }
    public Guid ProjectTaskId { get; set; } 
    public List<CommentFileModel> Files { get; set; }
}