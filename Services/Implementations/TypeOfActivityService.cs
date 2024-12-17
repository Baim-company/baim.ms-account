using Global.Infrastructure.Exceptions.PersonalAccount;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Services.Abstractions;
using PersonalAccount.API.Data.DbContexts;
using Microsoft.EntityFrameworkCore;


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



    public async Task<List<string>> GetTypesAsync()
    {
        var types = await _agileDbContext.TypeOfActivities
            .AsNoTracking()
            .ToListAsync();

        if (types == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound, "Error!Failed to get type of activities");

        List<string> typesName = new List<string>();
        foreach (var type in types)
            typesName.Add(type.Title);

        return typesName;
    }



    public async Task<Response<TypeOfActivity>> AddTypeAsync(string title)
    {
        var type = await _agileDbContext.TypeOfActivities
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Title == title);
        if (type != null) return new Response<TypeOfActivity>($"Error!Type of activity: {title} already exist!");


        TypeOfActivity typeOfActivity = new(title);

        await _agileDbContext.TypeOfActivities.AddAsync(typeOfActivity);
        await _agileDbContext.SaveChangesAsync();

        return new Response<TypeOfActivity>("Successfully created!", typeOfActivity);
    }




    public async Task<Response<List<TypeOfActivity>>> AddTypesRangeAsync(List<string> titles)
    {
        List<TypeOfActivity> typeOfActivities = new();
        foreach (var title in titles)
        {
            var type = await _agileDbContext.TypeOfActivities
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Title == title);
            if (type != null) return new Response<List<TypeOfActivity>>($"Error! Failed to create type of activity with title: {title}. Already exist!");

            typeOfActivities.Add(new TypeOfActivity(title));
        }

        await _agileDbContext.TypeOfActivities.AddRangeAsync(typeOfActivities);
        await _agileDbContext.SaveChangesAsync();

        return new Response<List<TypeOfActivity>>("Successfully added .", typeOfActivities);
    }




    public async Task<Response<TypeOfActivity>> UpdateTypeAsync(Guid id, string title)
    {
        var type = await _agileDbContext.TypeOfActivities.FindAsync(id);
        if (type == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound,
            $"Error!Type of activity with id: {id} doesn't exist!");

        await _agileDbContext.TypeOfActivities
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(u => u
                .SetProperty(u => u.Title, title));

        await _agileDbContext.SaveChangesAsync();

        return new Response<TypeOfActivity>("Type of activity updated successfully!", new TypeOfActivity());
    }



    public async Task<Response<TypeOfActivity>> DeleteTypeAsync(Guid id)
    {
        var type = await _agileDbContext.TypeOfActivities.FindAsync(id);
        if (type == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound,
            $"Error!Type of activity with id: {id} doesn't exist!");

        await _agileDbContext.TypeOfActivities
            .Where(v => v.Id == id)
            .ExecuteDeleteAsync();
        await _agileDbContext.SaveChangesAsync();

        return new Response<TypeOfActivity>("Successfully deleted!", new TypeOfActivity());
    }
}