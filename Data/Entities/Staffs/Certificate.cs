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


    [JsonIgnore]
    public byte[]? CertificateFile { get; set; } 
    [JsonIgnore]
    public string CertificateFileType { get; set; } = "data:image/png;base64,";
    public string CombinedImage => CertificateFile != null && CertificateFile.Length > 0 ? $"{CertificateFileType}{Convert.ToBase64String(CertificateFile)}" : CertificateFileType;



    public Certificate()
    {
        Id = Guid.NewGuid();
    }

    public Certificate(CertificateModel certificateModel,Staff staff)
    {
        Id = Guid.NewGuid();

        if (!string.IsNullOrEmpty(certificateModel.CertificateFileStr))
        {
            CertificateFile = Convert.FromBase64String(certificateModel.CertificateFileStr.Split(',')[1]);
            CertificateFileType = certificateModel.CertificateFileStr.Split(',')[0] + ",";
        }
        Name = certificateModel.Name;
        Authority = certificateModel.Authority;
        GivenTime = certificateModel.GivenTime;
        Deadline = certificateModel.Deadline;
        Link = certificateModel.Link;

        Staff = staff;
        StaffId = staff.Id;
    }
}