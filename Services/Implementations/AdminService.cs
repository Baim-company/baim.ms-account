using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Staffs;
using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly AgileDbContext _agileDbContext;

    public AdminService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }


    public async Task<Response<Staff>> AddAdminAsync(ExternalUserDto externalUserDto)
    {
        var userExist = await _agileDbContext.Users.FindAsync(externalUserDto.Id);
        if (userExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.UserNotFound, $"Error! There are already exist user with user id: {externalUserDto.Id}");


        var staffExist = await _agileDbContext.Staff
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == externalUserDto.Id);
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
}