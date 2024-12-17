namespace PersonalAccount.API.Models.Dtos.Responses;

public class SimpleApiResponse
{
    public string Message { get; set; }
    public bool Success { get; set; }

    public SimpleApiResponse(string message, bool success = true)
    {
        Message = message;
        Success = success;
    }
}