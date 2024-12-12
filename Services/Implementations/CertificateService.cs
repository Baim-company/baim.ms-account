using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class CertificateService : ICertificateService
{
    private readonly AgileDbContext _agileDbContext;

    public CertificateService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }


    public async Task<Response<Certificate>> AddCertificatesAsync(Guid staffId, List<CertificateModel> certificateModels)
    {
        try
        { 
            var staff = await _agileDbContext.Staff.FindAsync(staffId);

            if (staff == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!nStaff with id: {staffId} doesn't exist!");


            List<Certificate> certificates = new();
            foreach (var certificateModel in certificateModels)
            {
                var staffcertificate = new Certificate(certificateModel, staff);
                certificates.Add(staffcertificate);
            }

            await _agileDbContext.Certificates.AddRangeAsync(certificates);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Certificate>("Certificate(s) successfully added", new Certificate());
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Response<Certificate>> DeleteCertificatesAsync(Guid id)
    {
        try
        {
            var certificateExist = await _agileDbContext.Certificates.FindAsync(id);
            if (certificateExist == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.SerteficateNotFound, $"Error!Certificate with id: {id} doesn't exist!");

            await _agileDbContext.Certificates
                .Where(s => s.Id == id)
                .ExecuteDeleteAsync();
            await _agileDbContext.SaveChangesAsync();

            return new Response<Certificate>("Successfully deleted!", new Certificate());
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<List<Certificate>> GetCertificatesAsync(Guid staffId)
    {
        try
        {
            var certificates = await _agileDbContext.Certificates.AsNoTracking()
                .Where(s => s.StaffId == staffId).ToListAsync();

            if (certificates == null || certificates.Count == 0) return new List<Certificate>();

            return certificates;
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<Certificate> GetCertificateAsync(Guid id)
    {
        try
        {
            var certificate = await _agileDbContext.Certificates.FindAsync(id);

            if (certificate == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.SerteficateNotFound, $"Error!Certificate with id: {id} doesn't exist!");

            return certificate;
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Response<Certificate>> UpdateCertificatesAsync(List<UpdateCertificateModel> certificateModels)
    {
        try
        {
            if (certificateModels == null || certificateModels.Count == 0)
                return new Response<Certificate>("Error!Certificate models are null or empty here", null);

            foreach (var certificateModel in certificateModels)
            {
                var certificate = await _agileDbContext.Certificates
                            .Include(s => s.Staff)
                            .Where(s => s.Id == certificateModel.Id)
                            .FirstOrDefaultAsync();
                if (certificate == null)
                    throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Error!Failed to get certificate with staff id: {certificateModel.Id}");


                byte[]? certificateFile = null;
                string certificateFileType = "data:image/png;base64,";
                if (!string.IsNullOrEmpty(certificateModel.CertificateFileStr))
                {
                    certificateFile = Convert.FromBase64String(certificateModel.CertificateFileStr.Split(',')[1]);
                    certificateFileType = certificateModel.CertificateFileStr.Split(',')[0] + ",";
                }


                certificate.Name = certificateModel.Name;
                certificate.Authority = certificateModel.Authority;
                certificate.CertificateFile = certificateFile;
                certificate.CertificateFileType = certificateFileType;
                certificate.Link = certificateModel.Link;
                certificate.GivenTime = certificateModel.GivenTime;
                certificate.Deadline = certificateModel.Deadline;

                _agileDbContext.Certificates.Update(certificate);
                await _agileDbContext.SaveChangesAsync();
            }

            return new Response<Certificate>("Certificate successfully updated", new Certificate());
        }
        catch (Exception)
        {
            throw;
        }
    }
}