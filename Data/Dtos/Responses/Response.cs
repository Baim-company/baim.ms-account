namespace PersonalAccount.API.Models.Dtos.Responses;
public record Response<T>
{
    public string Message { get; set; } = "Error!";
    public T? Data { get; set; }

    public Response(string message)
    {
        Message = message;
    }
    public Response(string message, T? data)
    {
        Message = message;
        Data = data;
    }
}