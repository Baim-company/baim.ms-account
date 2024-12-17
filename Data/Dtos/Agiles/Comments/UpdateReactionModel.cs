namespace PersonalAccount.API.Models.Dtos.Agiles.Comments;

public class UpdateReactionModel
{
    public Guid ReactionId { get; set; }
    public string NewEmoji { get; set; }
}