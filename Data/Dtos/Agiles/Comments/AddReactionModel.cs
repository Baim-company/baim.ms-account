namespace PersonalAccount.API.Models.Dtos.Agiles.Comments
{
    public class AddReactionModel
    {
        public string Emoji { get; set; }  
        public Guid UserId { get; set; }
    }
}
