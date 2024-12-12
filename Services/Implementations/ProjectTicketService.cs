using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class ProjectTicketService: IProjectTicketService
{
    private readonly AgileDbContext _agileDbContext;

    public ProjectTicketService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }


    public async Task<Response<ProjectTiket>> CreateProjectTicketAsync(ProjectTiketModel projectTiketModel)
    {
        try
        {
            var isProjectTicketExist = await _agileDbContext.Projects
                .AnyAsync(p => p.Id == projectTiketModel.ProjectId
                && p.ProjectTikets!.Any(p => p.Title.ToLower() == projectTiketModel.Title.ToLower()));

            if (isProjectTicketExist)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketAlreadyExists,
                    $"Project ticket with project id: {projectTiketModel.ProjectId} and title: {projectTiketModel.Title} already exists!");

            ProjectTiket projectTiket = new(projectTiketModel.ProjectId, projectTiketModel.Title);
            List<ProjectTiketUser> projectTiketUsers = new();

            foreach (var userId in projectTiketModel.projectUserIds!)
            {
                var userExists = await _agileDbContext.ProjectUsers.AnyAsync(pu => pu.Id == userId);
                if (!userExists)
                    throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketUserNotFound,
                        $"Project user with user id: {userId} doesn't exist!");

                ProjectTiketUser user = new() { ProjectTiketId = projectTiket.Id, ProjectUserId = userId };
                projectTiketUsers.Add(user);
            }

            await _agileDbContext.ProjectTikets.AddAsync(projectTiket);
            await _agileDbContext.ProjectTiketUsers.AddRangeAsync(projectTiketUsers);
            await _agileDbContext.SaveChangesAsync();

            return new Response<ProjectTiket>("Successfully added!", projectTiket);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<ProjectTiket>> DeleteProjectTicketAsync(Guid id)
    {
        try
        {
            var projectTicket = await _agileDbContext.ProjectTikets.FindAsync(id);
            if (projectTicket == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketNotFound,
                    $"Project ticket with id: {id} doesn't exist!");

            await _agileDbContext.ProjectTiketUsers
                .Where(pt => pt.ProjectTiketId == id)
                .ExecuteDeleteAsync();

            await _agileDbContext.ProjectTikets
                .Where(pt => pt.Id == id)
                .ExecuteDeleteAsync();

            await _agileDbContext.SaveChangesAsync();
            return new Response<ProjectTiket>("Successfully deleted!", new ProjectTiket());
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<ProjectTiket?> GetProjectTicketByIdAsync(Guid id)
    {
        try
        {
            var projectTicket = await _agileDbContext.ProjectTikets
                .Include(pt => pt.ProjectTiketUsers)
                    .ThenInclude(ptu => ptu.ProjectUser)
                        .ThenInclude(pt => pt.Client)
                            .ThenInclude(pt => pt!.User)
                .Include(pt => pt.ProjectTiketUsers)
                    .ThenInclude(ptu => ptu.ProjectUser)
                        .ThenInclude(pt => pt.Staff)
                            .ThenInclude(pt => pt!.User)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (projectTicket == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketNotFound,
                    "Project ticket not found");

            return projectTicket;
        }
        catch (Exception)
        {
            throw;
        }
    }




    public async Task<List<ProjectTiket>?> GetProjectTicketsAsync()
    {
        try
        {
            var projectTickets = await _agileDbContext.ProjectTikets
                .Include(pt => pt.ProjectTiketUsers)
                    .ThenInclude(ptu => ptu.ProjectUser)
                        .ThenInclude(pt => pt.Client)
                            .ThenInclude(pt => pt!.User)
                .Include(pt => pt.ProjectTiketUsers)
                    .ThenInclude(ptu => ptu.ProjectUser)
                        .ThenInclude(pt => pt.Staff)
                            .ThenInclude(pt => pt!.User)
                .ToListAsync();

            return projectTickets ?? new List<ProjectTiket>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<ProjectTiket>> UpdateProjectTicketAsync(UpdateProjectTiketModel updateProjectTiket)
    {
        try
        {
            var isProjectTicketExist = await _agileDbContext.Projects
                .AnyAsync(p => p.Id == updateProjectTiket.ProjectId
                && p.ProjectTikets!.Any(p => p.Title.ToLower() == updateProjectTiket.Title.ToLower()));

            if (isProjectTicketExist)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketAlreadyExists,
                    $"Project ticket with project id: {updateProjectTiket.ProjectId} and title: {updateProjectTiket.Title} already exists!");

            await _agileDbContext.ProjectTiketUsers
                .Where(ptu => ptu.ProjectTiketId == updateProjectTiket.Id)
                .ExecuteDeleteAsync();

            List<ProjectTiketUser> projectTiketUsers = new();

            foreach (var userId in updateProjectTiket.projectUserIds!)
            {
                var userExists = await _agileDbContext.ProjectUsers.AnyAsync(pu => pu.Id == userId);
                if (!userExists)
                    throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketUserNotFound,
                        $"Project user with user id: {userId} doesn't exist!");

                ProjectTiketUser user = new() { ProjectTiketId = updateProjectTiket.Id, ProjectUserId = userId };
                projectTiketUsers.Add(user);
            }

            await _agileDbContext.ProjectTikets
                .Where(pt => pt.Id == updateProjectTiket.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.Deadline, updateProjectTiket.Deadline)
                    .SetProperty(u => u.Title, updateProjectTiket.Title));

            await _agileDbContext.ProjectTiketUsers.AddRangeAsync(projectTiketUsers);
            await _agileDbContext.SaveChangesAsync();

            return new Response<ProjectTiket>("Successfully updated!", new ProjectTiket());
        }
        catch (Exception)
        {
            throw;
        }
    }
}