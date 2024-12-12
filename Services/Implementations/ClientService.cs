using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Entities.Users;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class ClientService : IClientService
{

    private readonly AgileDbContext _agileDbContext;

    public ClientService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }


    public async Task<Response<Client>> GetClientByUserIdAsync(Guid userId)
    {
        var client = await _agileDbContext.Clients
            .Include(c => c.Company)
            .Include(c => c.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(r => r.Role)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (client == null)
            throw new PersonalAccountException(PersonalAccountErrorType.ClientNotFound, $"Client with user id: {userId} does not exist!");

        return new Response<Client>("Success", client);
    }


    public async Task<Response<Client>> GetClientAsync(Guid id)
    {
        try
        {
            var client = await _agileDbContext.Clients
                .Include(c => c.Company)
                .Include(c => c.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(r => r.Role)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ClientNotFound, $"Client with id: {id} does not exist!");

            return new Response<Client>("Success", client);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<PagedResponse<Client>> GetFilteredClientsAsync(PaginationParameters paginationParameters, string? onFilter)
    {
        try
        {
            var clientsQuery = _agileDbContext.Clients
                .Include(c => c.Company)
                .Include(c => c.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(r => r.Role)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(onFilter))
            {
                clientsQuery = clientsQuery.Where(client =>
                    (client.User.Name != null && client.User.Name.ToLower().Contains(onFilter.ToLower())) ||
                    (client.User.Surname != null && client.User.Surname.ToLower().Contains(onFilter.ToLower())) ||
                    (client.User.Email != null && client.User.Email.ToLower().Contains(onFilter.ToLower()))
                );
            }

            var totalRecords = await clientsQuery.CountAsync();
            var paginatedClients = await clientsQuery
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            return new PagedResponse<Client>(paginatedClients, paginationParameters.PageNumber, paginationParameters.PageSize, totalRecords);
        }
        catch (Exception)
        {
            throw;
        }
    }




    public async Task<Response<Client>> AddClientAdminAsync(ExternalUserDto externalUserDto)
    {
        var userExist = await _agileDbContext.Users.FindAsync(externalUserDto.Id);
        if (userExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.UserAlreadyExists, $"User with id: {userExist.Id} already exists!");


        var clientExist = await _agileDbContext.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == externalUserDto.Id);
        if (clientExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.ClientAlreadyExists, $"Client with user id: {externalUserDto.Id} already exists!");


        User user = new User(externalUserDto);
        Role newRole = new Role(externalUserDto.Role);
        UserRole userRole = new UserRole(user, newRole);
        Company company = new Company();
        Client newClient = new Client(user, company);

        await _agileDbContext.Users.AddAsync(user);
        await _agileDbContext.Roles.AddAsync(newRole);
        await _agileDbContext.Companies.AddAsync(company);
        await _agileDbContext.UserRoles.AddAsync(userRole);
        await _agileDbContext.Clients.AddAsync(newClient);

        await _agileDbContext.SaveChangesAsync();

        return new Response<Client>("Successfully created!", newClient);
    }

    public async Task<Response<Client>> AddClientAsync(ExternalUserDto externalUserDto, Guid companyId)
    {
        var userExist = await _agileDbContext.Users.FindAsync(externalUserDto.Id);
        if (userExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.UserAlreadyExists, $"User with id: {userExist.Id} already exists!");


        var clientExist = await _agileDbContext.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == externalUserDto.Id);
        if (clientExist != null)
            throw new PersonalAccountException(PersonalAccountErrorType.ClientAlreadyExists, $"Client with user id: {externalUserDto.Id} already exists!");


        var companyExist = await _agileDbContext.Companies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == companyId);
        if (companyExist == null)
            throw new PersonalAccountException(PersonalAccountErrorType.CompanyNotFound, $"Company with id: {companyId} doesn't exist!");


        User user = new User(externalUserDto);
        Role newRole = new Role(externalUserDto.Role);
        UserRole userRole = new UserRole(user, newRole);
        Client newClient = new Client(user)
        {
            CompanyId = companyId
        };

        await _agileDbContext.Users.AddAsync(user);
        await _agileDbContext.UserRoles.AddAsync(userRole);
        await _agileDbContext.Roles.AddAsync(newRole);
        await _agileDbContext.Clients.AddAsync(newClient);

        await _agileDbContext.SaveChangesAsync();

        return new Response<Client>("Successfully created!", newClient);
    }



    public async Task<Response<Client>> UpdateClientDataAsync(UpdateUserModel updateUserModel)
    {
        var client = await _agileDbContext.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.UserId == updateUserModel.Id);
        if (client == null)
            throw new PersonalAccountException(PersonalAccountErrorType.ClientNotFound, $"Client with user id: {updateUserModel.Id} doesn't exist!");



        if (client.User.Email != updateUserModel.Email &&
            await _agileDbContext.Users.AnyAsync(u => u.Email == updateUserModel.Email))
            throw new PersonalAccountException(PersonalAccountErrorType.EmailAlreadyExists, "Email already exists!");

        await _agileDbContext.Users
            .Where(u => u.Id == updateUserModel.Id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Name, updateUserModel.Name)
                .SetProperty(u => u.Surname, updateUserModel.Surname)
                .SetProperty(u => u.Patronymic, updateUserModel.Patronymic)
                .SetProperty(u => u.Position, updateUserModel.Position)
                .SetProperty(u => u.Gender, updateUserModel.Gender)
                .SetProperty(u => u.BirthDate, updateUserModel.BirthDate)
                .SetProperty(u => u.Email, updateUserModel.Email)
                .SetProperty(u => u.PersonalEmail, updateUserModel.PersonalEmail)
                .SetProperty(u => u.PhoneNumber, updateUserModel.PhoneNumber)
                .SetProperty(u => u.BusinessPhoneNumber, updateUserModel.BusinessPhoneNumber));

        await _agileDbContext.SaveChangesAsync();

        return new Response<Client>("Successfully updated!", client);
    }



    public async Task<Response<Client>> UpdateClientActiveStatusAsync(Guid id)
    {
        var client = await _agileDbContext.Clients
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
            throw new PersonalAccountException(PersonalAccountErrorType.ClientNotFound, $"Client with id: {id} does not exist!");

        await _agileDbContext.Clients
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(c => c.SetProperty(c => c.IsActive, !client.IsActive));

        await _agileDbContext.SaveChangesAsync();

        return new Response<Client>("Successfully updated!", client);
    }

}