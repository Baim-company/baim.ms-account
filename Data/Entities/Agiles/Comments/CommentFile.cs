using System.Text.Json.Serialization;

namespace PersonalAccount.API.Models.Entities.Agiles.Comments;

public class CommentFile
{
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string FileExtension { get; set; }
    public byte[] FileData { get; set; }

    [JsonIgnore]
    public Guid CommentId { get; set; }

    [JsonIgnore]
    public Comment Comment { get; set; }
}