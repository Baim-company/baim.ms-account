using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Data.Dtos.Staffs;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Dtos.Staffs;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class StaffService : IStaffService
{
    private readonly AgileDbContext _agileDbContext;

    public StaffService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }



    public async Task<Response<Staff>> GetStaffAsync(Guid id)
    {
        var staff = await _agileDbContext.Staff.FindAsync(id);

        if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.StaffNotFound, $"Staff with this id: {id} doesn't exist!");

        return new Response<Staff>($"Successfully received", staff);
    }




    public async Task<PagedResponse<Staff>> GetFilteredStaffAsync(PaginationParameters paginationParameters, string? orFilter, string? orPosition)
    {
        var staffQuery = _agileDbContext.Staff
            .Include(s => s.StaffImages)
            .Include(s => s.Certificates)
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

        return new PagedResponse<Staff>(paginatedStaff, paginationParameters.PageNumber, paginationParameters.PageSize, totalRecords);
    }





    public async Task<Response<Staff>> AddStaffAsync(ExternalUserDto externalUserDto)
    {
        var userExist = await _agileDbContext.Users.FindAsync(externalUserDto.Id);
        if (userExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"Error! There are already exist user with user id: {externalUserDto.Id}");


        var staffExist = await _agileDbContext.Staff
            .AsNoTracking()
            .Where(s => s.UserId == externalUserDto.Id)
            .FirstOrDefaultAsync();
        if (staffExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"Error! Staff with this user id: {externalUserDto.Id} already exist!");


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





    public async Task<Response<Staff>> ChangePositionAsync(UpdatePosition updatePosition)
    {
        var staff = await _agileDbContext.Staff
            .Include(c => c.User)
            .Where(c => c.UserId == updatePosition.Id)
            .FirstOrDefaultAsync();
        if (staff == null) 
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"Error!\nStaff with user id: {updatePosition.Id} doesn't exist!");

        await _agileDbContext.Users
            .Where(u => u.Id == updatePosition.Id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Position, updatePosition.Position));
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
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"Error!\nStaff with user id: {updateUserModel.Id} doesn't exist!");



        if (staff.User.Email != updateUserModel.Email
            && await _agileDbContext.Users.AnyAsync(u => u.Email == updateUserModel.Email))
            throw new PersonalAccountException(PersonalAccountErrorType.StaffAlreadyExists, $"Error!\nStaff with email already exist!");


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
        try
        {
            var staff = await _agileDbContext.Staff.FindAsync(id);
            if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"Error!\nStaff with id: {id} doesn't exist!");

            staff.Experience = experience;
            _agileDbContext.Staff.Update(staff);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Staff>("Successfully updated!", staff);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Response<Staff>> SetIsWorkingOrDismissedAsync(Guid id)
    {
        try
        {
            var staff = await _agileDbContext.Staff.FindAsync(id);
            if (staff == null) throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"Error!\nStaff with id: {id} doesn't exist!");

            staff.IsDismissed = !staff.IsDismissed;

            _agileDbContext.Staff.Update(staff);
            await _agileDbContext.SaveChangesAsync();

            return new Response<Staff>("Successfully updated!", staff);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<Response<List<StaffSummaryDto>>> GetAllStaffSortedByExperience()
    {
        try
        {
            var staffList = await _agileDbContext.Staff
                .Include(s => s.User)
                .Where(s => s.User.Email != "admin@gmail.com") 
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
                        CombinedImage = s.StaffImages?.FirstOrDefault()?.CombinedImage ?? null,
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


    private (int completedProjectsCount, int totalClientsCount)
        CalculateCompletedProjectsAndClients(Staff staff)
    {
        var completedProjectsCount = 0;
        var totalClientsCount = 0;

        if (staff.MyManageProjects != null)
        {
            foreach (var project in staff.MyManageProjects.Where(p => p.IsCompleted))
            {
                completedProjectsCount++;
                totalClientsCount += project.ProjectUsers.Count(pu => pu.Client != null);
            }
        }

        if (staff.ProjectUsers != null)
        {
            foreach (var projectUser in staff.ProjectUsers.Where(pu => pu.Project.IsCompleted))
            {
                completedProjectsCount++;
                totalClientsCount += projectUser.Project.ProjectUsers.Count(p => p.Client != null);
            }
        }

        return (completedProjectsCount, totalClientsCount);
    }

}