using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Dtos.Clients;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Clients;
using PersonalAccount.API.Services.Abstractions;

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
        try
        {
            var type = await _agileDbContext.TypeOfActivities.FindAsync(id);

            if (type == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound, $"Error!Failed to get type of activity with id: {id}");

            return new Response<TypeOfActivity>("Succsess!", type);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<List<TypeOfActivity>> GetTypesAsync()
    {
        try
        {
            var types = await _agileDbContext.TypeOfActivities
                .AsNoTracking()
                .ToListAsync();

            if (types == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound, "Error!Failed to get type of activities");

            return types;
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Response<TypeOfActivity>> AddTypeAsync(TypeOfActivityModel typeModel)
    {
        try
        {
            var type = await _agileDbContext.TypeOfActivities
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Title == typeModel.Title);
            if (type != null) return new Response<TypeOfActivity>($"Error!Type of activity: {typeModel.Title} already exist!", null);


            TypeOfActivity typeOfActivity = new(typeModel);

            await _agileDbContext.TypeOfActivities.AddAsync(typeOfActivity);
            await _agileDbContext.SaveChangesAsync();

            return new Response<TypeOfActivity>("Successfully created!", typeOfActivity);
        }
        catch (Exception)
        {
            throw;
        }
    }




    public async Task<(List<TypeOfActivity>?, TypeOfActivity?)> AddTypesRangeAsync(List<TypeOfActivityModel> typeModels)
    {
        try
        {
            List<TypeOfActivity> typeOfActivities = new();
            foreach (var typeModel in typeModels)
            {
                var type = await _agileDbContext.TypeOfActivities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Title == typeModel.Title);
                if (type != null) return new(null, type);

                typeOfActivities.Add(new TypeOfActivity(typeModel));
            }

            await _agileDbContext.TypeOfActivities.AddRangeAsync(typeOfActivities);
            await _agileDbContext.SaveChangesAsync();

            return new(typeOfActivities, null);
        }
        catch (Exception)
        {
            throw;
        }
    }




    public async Task<Response<TypeOfActivity>> UpdateTypeAsync(UpdateTypeOfActivityModel typeModel)
    {
        try
        {
            var type = await _agileDbContext.TypeOfActivities.FindAsync(typeModel.Id);
            if (type == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound, $"Error!Type of activity with id: {typeModel.Id} doesn't exist!");

            await _agileDbContext.TypeOfActivities
                .Where(u => u.Id == typeModel.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.Title, typeModel.Title)
                    .SetProperty(u => u.Id1C, typeModel.Id1C));

            await _agileDbContext.SaveChangesAsync();

            return new Response<TypeOfActivity>("Type of activity updated successfully!", new TypeOfActivity());
        }
        catch (Exception)
        {
            throw;
        }
    }

    

    public async Task<Response<TypeOfActivity>> DeleteTypeAsync(Guid id)
    {
        try
        {
            var type = await _agileDbContext.TypeOfActivities.FindAsync(id);
            if (type == null) throw new PersonalAccountException(PersonalAccountErrorType.TypeOfActivityNotFound, $"Error!Type of activity with id: {id} doesn't exist!");

            await _agileDbContext.TypeOfActivities
                .Where(v => v.Id == id)
                .ExecuteDeleteAsync();
            await _agileDbContext.SaveChangesAsync();

            return new Response<TypeOfActivity>("Successfully deleted!", new TypeOfActivity());
        }
        catch (Exception)
        {
            throw;
        }
    }
}