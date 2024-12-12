namespace PersonalAccount.API.Models.Enums;
public enum DoneState
{
    Pending,   // Ожидающий подтверждения,
    InProcess, // В процессе,
    Done,      // Сделано, 
    Cancelled  // Отмененный
}