using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class CompanyService : ICompanyService
{
    private readonly AgileDbContext _agileDbContext;
    public CompanyService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }



    public async Task<Response<Company>> CompleteCompanyAsync(CompanyModel companyModel)
    {
        try
        {
            var companyResponse = await _completeCompanyAsync(companyModel);
            if (companyResponse.Data == null) return new Response<Company>(companyResponse.Message, null);
            Company companyExist = companyResponse.Data;

            List<TypeOfActivity> typesOfActivities = new List<TypeOfActivity>();
            foreach (var typeId in companyModel.TypesOfActivities)
            {
                var typeExist = await _agileDbContext.TypeOfActivities.FindAsync(typeId);
                if (typeExist == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound, $"Error!Type of activity with this id: {typeId} not found!");
                typesOfActivities.Add(typeExist);
            }

            List<CompanyTypeOfActivity> companyTypesOfActivities = new List<CompanyTypeOfActivity>();
            foreach (var type in typesOfActivities)
            {
                companyTypesOfActivities.Add(new CompanyTypeOfActivity(companyExist, type));
            }

            _agileDbContext.Companies.Update(companyExist);

            await _agileDbContext.CompanyTypeOfActivities.AddRangeAsync(companyTypesOfActivities);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Company>("Company was successfully updated!", companyExist);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Company?> GetCompanyByIdAsync(Guid id)
    {
        try
        {
            var company = await _agileDbContext.Companies
                .Include(c => c.CompanyTypeOfActivities!)
                .ThenInclude(ct => ct.TypeOfActivity)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.CompanyNotFound, $"Company with id: {id} doesn't exist!");

            return company;
        }
        catch (Exception)
        {
            throw;
        }
    }




    public async Task<PagedResponse<Company>> GetFilteredCompaniesAsync(
        PaginationParameters paginationParameters,
        string? onSearch = null,
        string? onFilterProduct = null,
        string? onFilterTypeOfActivity = null,
        string? onFilterNational = null)
    {
        try
        {
            var companiesQuery = _agileDbContext.Companies
                .Include(c => c.CompanyTypeOfActivities!)
                .ThenInclude(c => c.TypeOfActivity)
                .Include(c => c.CompanyProducts!)
                .ThenInclude(c => c.Product)
                .Where(c => c.IsPublic == true)
                .AsNoTracking()
                .AsQueryable();

            // Фильтрация по onFilterSearch
            if (!string.IsNullOrEmpty(onSearch))
            {
                companiesQuery = companiesQuery.Where(company =>
                    company.CompanyName!.ToLower().Contains(onSearch.ToLower())
                );
            }
            // Фильтрация по onFilterProduct
            if (!string.IsNullOrEmpty(onFilterProduct))
            {
                companiesQuery = companiesQuery.Where(company =>
                    company.CompanyProducts!
                    .Any(ct => ct.Product.Name.ToLower() == onFilterProduct.ToLower()));
            }
            // Фильтрация по onFilterTypeOfActivity
            if (!string.IsNullOrEmpty(onFilterTypeOfActivity))
            {
                companiesQuery = companiesQuery.Where(company =>
                    company.CompanyTypeOfActivities!
                    .Any(ct => ct.TypeOfActivity.Title.ToLower() == onFilterTypeOfActivity.ToLower()));
            }
            // Фильтрация по onFilterNational
            if (!string.IsNullOrEmpty(onFilterNational))
            {
                var isNational = onFilterNational.ToLower() == "national";
                companiesQuery = companiesQuery.Where(company => company.IsNational == isNational);
            }


            var totalRecords = await companiesQuery.CountAsync();
            var paginatedCompanies = await companiesQuery
                    .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                    .Take(paginationParameters.PageSize)
                    .ToListAsync();

            return new PagedResponse<Company>(paginatedCompanies, paginationParameters.PageNumber, paginationParameters.PageSize, totalRecords);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Response<Company>> SetIsNationalOrPrivateAsync(Guid id)
    {
        try
        {
            var companyExist = await _agileDbContext.Companies.FindAsync(id);
            if (companyExist == null) throw new PersonalAccountException(PersonalAccountErrorType.CompanyNotFound, $"Error!\nCompany with id: {id} doesn't exist!");

            companyExist.IsNational = !companyExist.IsNational;
            _agileDbContext.Companies.Update(companyExist);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Company>("Successfully updated!", companyExist);
        }
        catch (Exception)
        {
            throw;
        }
    }




    public async Task<Response<Company>> ChangeIsPublicAsync(Guid id)
    {
        try
        {
            var companyExist = await _agileDbContext.Companies.FindAsync(id);
            if (companyExist == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.CompanyNotFound, $"Error!\nCompany with id: {id} doesn't exist!");

            companyExist.IsPublic = !companyExist.IsPublic;
            _agileDbContext.Companies.Update(companyExist);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Company>("Successfully updated!", companyExist);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Response<Company>> UpdateCompanyDataAsync(UpdateCompanyModel companyModel)
    { 
        try
        {
            var companyExist = await _agileDbContext.Companies
            .Include(c => c.CompanyTypeOfActivities!)
            .ThenInclude(ct => ct.TypeOfActivity)
            .FirstOrDefaultAsync(c => c.Id == companyModel.Id);

            if (companyExist == null) 
                throw new PersonalAccountException(PersonalAccountErrorType.CompanyNotFound, $"Error!\nCompany with id: {companyModel.Id} doesn't exist!");

            byte[]? image = null;
            string imageType = "data:image/png;base64,";
            if (!string.IsNullOrEmpty(companyModel.Logo))
            {
                image = Convert.FromBase64String(companyModel.Logo.Split(',')[1]);
                imageType = companyModel.Logo.Split(',')[0] + ",";
            }

            companyExist.Voen = companyModel.Voen;
            companyExist.CompanyName = companyModel.CompanyName;
            companyExist.LegalForm = companyModel.LegalForm;
            companyExist.LegalAddress = companyModel.LegalAddress;
            companyExist.LegalRepresentative = companyModel.LegalRepresentative;
            companyExist.IsNational = companyModel.IsNational;
            companyExist.Image = image;
            companyExist.ImageType = imageType;
            companyExist.IsPublic = true;

            if (companyModel.TypeOfActivityIds != null)
            { 
                _agileDbContext.CompanyTypeOfActivities
                    .RemoveRange(companyExist.CompanyTypeOfActivities!);
                 
                foreach (var typeId in companyModel.TypeOfActivityIds)
                {
                    var typeOfActivity = await _agileDbContext.TypeOfActivities.FindAsync(typeId);
                    if (typeOfActivity == null) continue;
                    companyExist.CompanyTypeOfActivities!.Add(new CompanyTypeOfActivity(companyExist, typeOfActivity));
                }
            }

            _agileDbContext.Companies.Update(companyExist);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Company>("Company updated successfully!", companyExist);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task<Response<Company>> _completeCompanyAsync(CompanyModel companyModel)
    {
        var companyExist = await _agileDbContext.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == companyModel.Id);
        if (companyExist == null)
            return new Response<Company>($"Error!Company with this id: {companyModel.Id} doesn't exist!", null);

        byte[]? image = null;
        string imageType = "data:image/png;base64,";
        if (!string.IsNullOrEmpty(companyModel.LogoImage))
        {
            image = Convert.FromBase64String(companyModel.LogoImage.Split(',')[1]);
            imageType = companyModel.LogoImage.Split(',')[0] + ",";
        }
        companyExist.IsNational = companyModel.IsNational;
        companyExist.Image = image;
        companyExist.ImageType = imageType;

        if (companyModel.voenModel == null) 
            return new Response<Company>("Error!Voen model is null!", null);

        companyExist.CompanyName = companyModel.voenModel.CompanyName;
        companyExist.Voen = companyModel.voenModel.Voen;
        companyExist.LegalForm = companyModel.voenModel.LegalForm;
        companyExist.LegalAddress = companyModel.voenModel.LegalAddress;
        companyExist.LegalRepresentative = companyModel.voenModel.LegalRepresentative;
        companyExist.IsPublic = true;

        return new Response<Company>("Success!", companyExist);
    }
}