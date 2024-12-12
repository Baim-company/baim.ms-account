using PersonalAccount.API.Models.Entities.Users;
using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.Comments;

public class Notification
{
    public Guid Id { get; set; }
    public string NotificationText { get; set; }
    public string CommentText { get; set; }
    public DateTime SentAt { get; set; } = DateTime.UtcNow.AddHours(4);

    [JsonIgnore]
    public Guid SenderId { get; set; }
    public User Sender { get; set; }

    public List<User> Recipients { get; set; } = new List<User>();

    public Notification()
    {
        Id = Guid.NewGuid();
        SentAt = DateTime.UtcNow.AddHours(4);
    }
}
