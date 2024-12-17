using Global.Infrastructure.Exceptions.PersonalAccount;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Services.Abstractions;
using PersonalAccount.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.Dtos.Clients;


namespace PersonalAccount.API.Services.Implementations;

public class TypeOfActivityService : ITypeOfActivityService
{
    private readonly AgileDbContext _agileDbContext;

    public TypeOfActivityService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }


    public async Task<Response<TypeOfActivity>> GetTypeAsync(Guid id)
    {
        var type = await _agileDbContext.TypeOfActivities.FindAsync(id);

        if (type == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound, $"Error!Failed to get type of activity with id: {id}");

        return new Response<TypeOfActivity>("Succsess!", type);
    }



    public async Task<List<TypeOfActivity>> GetTypesAsync()
    {
        var types = await _agileDbContext.TypeOfActivities
            .AsNoTracking()
            .ToListAsync();

        if (types == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound, "Error!Failed to get type of activities");

        return types;
    }



    public async Task<Response<TypeOfActivityDto>> AddTypeAsync(TypeOfActivityDto typeOfActivityDto)
    {
        var type = await _agileDbContext.TypeOfActivities
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Title == typeOfActivityDto.Title);
        if (type != null) return new Response<TypeOfActivityDto>($"Error!Type of activity: {typeOfActivityDto.Title} already exist!");


        TypeOfActivity typeOfActivity = new(typeOfActivityDto.Title);

        await _agileDbContext.TypeOfActivities.AddAsync(typeOfActivity);
        await _agileDbContext.SaveChangesAsync();

        return new Response<TypeOfActivityDto>("Successfully created!", new TypeOfActivityDto(typeOfActivityDto.Title));
    }




    public async Task<Response<List<TypeOfActivityDto>>> AddTypesRangeAsync(List<TypeOfActivityDto> typeOfActivityDtos)
    {
        List<TypeOfActivity> typeOfActivities = new();
        foreach (var typeOfActivityDto in typeOfActivityDtos)
        {
            var type = await _agileDbContext.TypeOfActivities
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Title == typeOfActivityDto.Title);
            if (type != null) return new Response<List<TypeOfActivityDto>>($"Error! Failed to create type of activity with title: {typeOfActivityDto.Title}. Already exist!");

            typeOfActivities.Add(new TypeOfActivity(typeOfActivityDto.Title));
        }

        await _agileDbContext.TypeOfActivities.AddRangeAsync(typeOfActivities);
        await _agileDbContext.SaveChangesAsync();

        return new Response<List<TypeOfActivityDto>>("Successfully added .", new List<TypeOfActivityDto>());
    }




    public async Task<Response<TypeOfActivityDto>> UpdateTypeAsync(Guid id, TypeOfActivityDto typeOfActivityDto)
    {
        var type = await _agileDbContext.TypeOfActivities.FindAsync(id);
        if (type == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound,
            $"Error!Type of activity with id: {id} doesn't exist!");

        await _agileDbContext.TypeOfActivities
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Title, typeOfActivityDto.Title));

        await _agileDbContext.SaveChangesAsync();

        return new Response<TypeOfActivityDto>("Type of activity updated successfully!", new TypeOfActivityDto());
    }



    public async Task<Response<TypeOfActivityDto>> DeleteTypeAsync(Guid id)
    {
        var type = await _agileDbContext.TypeOfActivities.FindAsync(id);
        if (type == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound,
            $"Error!Type of activity with id: {id} doesn't exist!");

        await _agileDbContext.TypeOfActivities
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();
        await _agileDbContext.SaveChangesAsync();

        return new Response<TypeOfActivityDto>("Successfully deleted!", new TypeOfActivityDto());
    }
}