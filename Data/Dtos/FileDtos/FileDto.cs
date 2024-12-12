namespace PersonalAccount.API.Data.Dtos.FileDtos;

public record FileDto
{
    public string FileName { get; set; }
    public string ContentType { get; set; }
    public byte[] FileContent { get; set; }
}