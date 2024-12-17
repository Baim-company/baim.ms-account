using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Data.Dtos.Staffs;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class StaffService : IStaffService
{

    private readonly string _baseImageUrl;
    private readonly IFileService _fileService;
    private readonly AgileDbContext _agileDbContext;

    public StaffService(AgileDbContext agileDbContext,
        IConfiguration configuration,
        IFileService fileService)
    {
        _fileService = fileService;
        _agileDbContext = agileDbContext;
        _baseImageUrl = configuration["BaseImageUrl"]
            ?? throw new Exception("BaseImageUrl is not configured");
    }



    public async Task<Response<Staff>> GetStaffAsync(Guid id)
    {
        var staff = await _agileDbContext.Staff
            .Include(s => s.User)
            .Include(s => s.StaffImages)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (staff == null) 
            throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Staff with this id: {id} doesn't exist!");

        staff.User.AvatarPath = $"{_baseImageUrl}/{staff.User.AvatarPath}".Replace("\\", "/");
        foreach (var staffImage in staff.StaffImages)
        {
            staffImage.ImagePath = $"{_baseImageUrl}/{staffImage.ImagePath}".Replace("\\", "/");
        }

        return new Response<Staff>($"Successfully received", staff);
    }




    public async Task<PagedResponse<Staff>> GetFilteredStaffAsync(PaginationParameters paginationParameters, string? orFilter, string? orPosition)
    {
        var staffQuery = _agileDbContext.Staff
            .Include(c => c.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(r => r.Role)
            .AsNoTracking()
            .AsQueryable();


        if (!string.IsNullOrEmpty(orFilter) && !string.IsNullOrEmpty(orPosition))
        {
            var filters = orFilter.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(f => f.ToLower());
            var positionFilter = orPosition.ToLower();
            staffQuery = staffQuery.Where(staff =>
                filters.Any(filter =>
                    (staff.User.Name != null && staff.User.Name.ToLower().Contains(filter)) ||
                    (staff.User.Surname != null && staff.User.Surname.ToLower().Contains(filter)) ||
                    (staff.User.Email != null && staff.User.Email.ToLower().Contains(filter))
                ) &&
                (staff.User.Position != null && staff.User.Position.ToLower().Contains(positionFilter))
            );
        }
        else if (!string.IsNullOrEmpty(orFilter))
        {
            var filters = orFilter.Split('|', StringSplitOptions.RemoveEmptyEntries).Select(f => f.ToLower());
            staffQuery = staffQuery.Where(staff =>
                filters.Any(filter =>
                    (staff.User.Name != null && staff.User.Name.ToLower().Contains(filter)) ||
                    (staff.User.Surname != null && staff.User.Surname.ToLower().Contains(filter)) ||
                    (staff.User.Email != null && staff.User.Email.ToLower().Contains(filter))
                )
            );
        }
        else if (!string.IsNullOrEmpty(orPosition))
        {
            var positionFilter = orPosition.ToLower();
            staffQuery = staffQuery.Where(staff =>
                staff.User.Position != null && staff.User.Position.ToLower().Contains(positionFilter)
            );
        }


        var totalRecords = await staffQuery.CountAsync();

        var paginatedStaff = await staffQuery
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync();

        foreach (var staff in paginatedStaff)
        {
            staff.User.AvatarPath = $"{_baseImageUrl}/{staff.User.AvatarPath}".Replace("\\", "/");
        }

        return new PagedResponse<Staff>(paginatedStaff, paginationParameters.PageNumber, paginationParameters.PageSize, totalRecords);
    }





    public async Task<Response<Staff>> AddStaffAsync(ExternalUserDto externalUserDto)
    {
        var userExist = await _agileDbContext.Users.FindAsync(externalUserDto.Id);
        if (userExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, 
                $"Error! There are already exist user with user id: {externalUserDto.Id}");


        var staffExist = await _agileDbContext.Staff
            .AsNoTracking()
            .Where(s => s.UserId == externalUserDto.Id)
            .FirstOrDefaultAsync();
        if (staffExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, 
                $"Error! Staff with this user id: {externalUserDto.Id} already exist!");


        User user = new User(externalUserDto);
        Role newRole = new Role(externalUserDto.Role);
        UserRole userRole = new UserRole(user, newRole);
        Staff newStaff = new Staff(user);

        await _agileDbContext.Users.AddAsync(user);
        await _agileDbContext.UserRoles.AddAsync(userRole);
        await _agileDbContext.Roles.AddAsync(newRole);
        await _agileDbContext.Staff.AddAsync(newStaff);

        await _agileDbContext.SaveChangesAsync();

        return new Response<Staff>("Successfully created!", newStaff);
    }





    public async Task<Response<Staff>> ChangePositionAsync(Guid id,string position)
    {
        var staff = await _agileDbContext.Staff
            .Include(c => c.User)
            .Where(c => c.UserId == id)
            .FirstOrDefaultAsync();
        if (staff == null) 
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, 
                $"Error!\nStaff with user id: {id} doesn't exist!");

        await _agileDbContext.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Position, position));
        await _agileDbContext.SaveChangesAsync();

        return new Response<Staff>("Successfully updated!", new Staff());
    }



    public async Task<Response<Staff>> UpdateStaffDataAsync(UpdateUserModel updateUserModel)
    {
        var staff = await _agileDbContext.Staff
            .Include(c => c.User)
            .Where(c => c.UserId == updateUserModel.Id)
            .FirstOrDefaultAsync();
        if (staff == null) 
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, 
                $"Error!\nStaff with user id: {updateUserModel.Id} doesn't exist!");


        if (staff.User.Email != updateUserModel.Email
            && await _agileDbContext.Users.AnyAsync(u => u.Email == updateUserModel.Email))
            throw new PersonalAccountException(PersonalAccountErrorType.StaffAlreadyExists, 
                $"Error!\nStaff with email already exist!");


        await _agileDbContext.Users
            .Where(u => u.Id == updateUserModel.Id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Name, updateUserModel.Name)
                .SetProperty(u => u.Surname, updateUserModel.Surname)
                .SetProperty(u => u.Patronymic, updateUserModel.Patronymic)
                .SetProperty(u => u.Gender, updateUserModel.Gender)
                .SetProperty(u => u.BirthDate, updateUserModel.BirthDate)
                .SetProperty(u => u.Email, updateUserModel.Email)
                .SetProperty(u => u.PersonalEmail, updateUserModel.PersonalEmail)
                .SetProperty(u => u.PhoneNumber, updateUserModel.PhoneNumber)
                .SetProperty(u => u.BusinessPhoneNumber, updateUserModel.BusinessPhoneNumber));
        await _agileDbContext.SaveChangesAsync();

        return new Response<Staff>("Successfully updated!", staff);
    }



    public async Task<Response<Staff>> SetStaffExperienceAsync(Guid id, ushort experience)
    {
            var staff = await _agileDbContext.Staff.FindAsync(id);
            if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, 
                $"Error!\nStaff with id: {id} doesn't exist!");

            staff.Experience = experience;
            _agileDbContext.Staff.Update(staff);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Staff>("Successfully updated!", staff);
    }



    public async Task<Response<Staff>> SetIsWorkingOrDismissedAsync(Guid id)
    {
            var staff = await _agileDbContext.Staff.FindAsync(id);
            if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, 
                $"Error!\nStaff with id: {id} doesn't exist!");

            staff.IsDismissed = !staff.IsDismissed;

            _agileDbContext.Staff.Update(staff);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Staff>("Successfully updated!", staff);
    }





    /// <summary>
    /// Не понимаю зачем эти методы
    /// </summary>
    /// <returns></returns>
    public async Task<Response<List<StaffSummaryDto>>> GetAllStaffSortedByExperience()
    {
        try
        {
            var staffList = await _agileDbContext.Staff
                .Include(s => s.User)
                .Where(s => s.User.Email != "admin@gmail.com" && !s.IsDismissed) 
                .Include(s => s.MyManageProjects)
                .Include(s => s.ProjectUsers)
                .ThenInclude(pu => pu.Project)
                .Include(s => s.StaffImages)
                .ToListAsync();

            var staffSummaries = staffList
                .Select(s =>
                {
                    var (completedProjectsCount, totalClientsCount) = CalculateCompletedProjectsAndClients(s);
                    return new StaffSummaryDto
                    {
                        Id = s.Id,
                        FirstName = s.User.Name,
                        LastName = s.User.Surname,
                        Position = s.User.Position,
                        Experience = s.Experience,
                        CombinedImage = s.StaffImages?.FirstOrDefault()?.ImagePath ?? null,
                        TotalCompletedProjectsCount = completedProjectsCount,
                        TotalClientsInCompletedProjects = totalClientsCount
                    };
                })
                .OrderByDescending(s => s.Experience)
                .ToList();

            return new Response<List<StaffSummaryDto>>("Success", staffSummaries);
        }
        catch (Exception ex)
        {
            return new Response<List<StaffSummaryDto>>($"Failed to retrieve staff data: {ex.Message}");
        }
    }




    /// <summary>
    /// Не понимаю зачем эти методы
    /// </summary>
    /// <returns></returns>
    public async Task<Response<StaffDetailsDto>> GetStaffDetailsByIdAsync(Guid staffId)
    {
        try
        {
            var staff = await _agileDbContext.Staff
                .Include(s => s.User)
                .Include(s => s.StaffImages.Where(img => img.IsPageImage))
                .Include(s => s.MyManageProjects)
                    .ThenInclude(p => p.Company)
                .Include(s => s.ProjectUsers)
                    .ThenInclude(pu => pu.Project)
                    .ThenInclude(p => p.Company)
                .Include(s => s.Certificates)
                .Where(s => !s.IsDismissed && s.Id == staffId)
                .FirstOrDefaultAsync();

            if (staff == null || staff.User == null)
            {
                return new Response<StaffDetailsDto>("Staff not found.");
            }

            var managedProjects = staff.MyManageProjects?
                .Where(p => p != null && p.IsCompleted && p.IsPublic)
                .ToList() ?? new List<Project>();

            var participatedProjects = staff.ProjectUsers?
                .Where(pu => pu.Project != null && pu.Project.IsCompleted && pu.Project.IsPublic)
                .Select(pu => pu.Project)
                .ToList() ?? new List<Project>();

            var allProjects = managedProjects
                .Concat(participatedProjects)
                .Distinct()
                .ToList();

            var completedProjectsCount = allProjects.Count;
            var totalClientsCount = allProjects
                .Sum(p => p.ProjectUsers?.Count(pu => pu.Client != null) ?? 0);

            var staffDetails = new StaffDetailsDto
            {
                Id = staff.Id,
                Name = staff.User.Name ?? "N/A",
                Surname = staff.User.Surname ?? "N/A",
                Position = staff.User.Position ?? "N/A",
                Experience = staff.Experience,

                StaffImages = staff.StaffImages?
                    .Where(img => img.IsPageImage)
                    .Select(img => img.ImagePath)
                    .ToList() ?? new List<string>(),

                CompletedProjectsCount = completedProjectsCount,
                ClientsInCompletedProjectsCount = totalClientsCount,

                Projects = allProjects.Select(p => new ProjectDetailsDto
                {
                    Name = p.Name ?? "No Name",
                    Description = p.Description ?? "No Description",
                    CompanyName = p.Company?.CompanyName ?? "Unknown Company",
                    CompanyImage = p.Company?.LogoImagePath ?? string.Empty
                }).ToList(),

                Sertificates = staff.Certificates?
                    .Select(c => new CertificateDetailsDto
                    {
                        Name = c.Name ?? "No Name",
                        Authority = c.Authority ?? "Unknown Authority",
                        GivenTime = c.GivenTime,
                        Deadline = c.Deadline,
                        Link = c.Link ?? string.Empty,
                        CertificateFilePath = c.CertificateFilePath ?? string.Empty
                    }).ToList() ?? new List<CertificateDetailsDto>()
            };

            return new Response<StaffDetailsDto>("Success", staffDetails);
        }
        catch (Exception ex)
        {
            return new Response<StaffDetailsDto>($"Failed to retrieve staff details: {ex.Message}");
        }
    }




    /// <summary>
    /// Не понимаю зачем эти методы
    /// </summary>
    /// <returns></returns>
    private (int completedProjectsCount, int totalClientsCount)
        CalculateCompletedProjectsAndClients(Staff staff)
    {
        var completedProjectsCount = 0;
        var totalClientsCount = 0;

        if (staff.MyManageProjects != null)
        {
            foreach (var project in staff.MyManageProjects.Where(p => p.IsCompleted  && p.IsPublic))
            {
                completedProjectsCount++;
                totalClientsCount += project.ProjectUsers.Count(pu => pu.Client != null);
            }
        }

        if (staff.ProjectUsers != null)
        {
            foreach (var projectUser in staff.ProjectUsers.Where(pu => pu.Project.IsCompleted && pu.Project.IsPublic))
            {
                completedProjectsCount++;
                totalClientsCount += projectUser.Project.ProjectUsers.Count(p => p.Client != null);
            }
        }

        return (completedProjectsCount, totalClientsCount);
    }
}