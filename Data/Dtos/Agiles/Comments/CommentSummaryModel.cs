using PersonalAccount.API.Models.Entities.Users;

namespace PersonalAccount.API.Models.Dtos.Agiles.Comments
{
    public class CommentSummaryModel
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateOfPublished { get; set; }


        public Guid FromUserId { get; set; }
        public User FromUser { get; set; }
    }
}
