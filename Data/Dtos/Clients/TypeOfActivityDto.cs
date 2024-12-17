namespace PersonalAccount.API.Data.Dtos.Clients;

public record TypeOfActivityDto{
    public string Title { get; set; }
    public TypeOfActivityDto(string title)
    {
        Title = title;
    }
    public TypeOfActivityDto() { }
}