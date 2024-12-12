namespace PersonalAccount.API.Models.Dtos.Agiles.Comments
{
    public class NotificationModel
    {
        public string NotificationText { get; set; }
        public string CommentText { get; set; }
        public DateTime SentAt { get; set; }
        public string SenderName { get; set; }
        public string[] AttachedFileNames { get; set; }
        public string[] AttachedFileExtensions { get; set; }
    }
}
