using System.Text.Json.Serialization;
using PersonalAccount.API.Models.Dtos.Staffs;

namespace PersonalAccount.API.Models.Entities.Staffs;

public class Certificate
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Authority { get; set; }
    public DateTime GivenTime { get; set; } = DateTime.UtcNow.AddHours(4);
    public DateTime Deadline { get; set; }
    public string Link { get; set; }


    [JsonIgnore]
    public Staff Staff { get; set; }
    [JsonIgnore]
    public Guid StaffId { get; set; }


    public string CertificateFilePath { get; set; } = "";



    public Certificate()
    {
        Id = Guid.NewGuid();
    }

    public Certificate(CertificateModel certificateModel,Staff staff)
    {
        Id = Guid.NewGuid();

        Name = certificateModel.Name;
        Authority = certificateModel.Authority;
        GivenTime = certificateModel.GivenTime;
        Deadline = certificateModel.Deadline;
        Link = certificateModel.Link;

        Staff = staff;
        StaffId = staff.Id;

        CertificateFilePath = certificateModel.CertificateFilePath;
    }
}