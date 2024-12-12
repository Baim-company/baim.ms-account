namespace PersonalAccount.API.Models.Dtos.Staffs;
public record StaffModel
{
    public Guid UserId { get; set; }
    public bool IsDismissed { get; set; }

    public ushort Experience { get; set; } = 0;
    public List<CertificateModel> CertificateModels { get; set; } = new List<CertificateModel>();
}