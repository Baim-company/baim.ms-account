using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.SubTickets;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class ProjectSubTicketService : IProjectSubTicketService
{
    private readonly AgileDbContext _agileDbContext;

    public ProjectSubTicketService(AgileDbContext agileDbContext)
    {
        _agileDbContext = agileDbContext;
    }

    public async Task<Response<ProjectSubTiket>> CreateProjectSubTicketAsync(ProjectSubTiketModel model)
    {
        try
        {
            var isProjectSubTicketExist = await _agileDbContext.ProjectTikets
                .AnyAsync(p => p.Id == model.ProjectTicketId
                               && p.ProjectSubTikets!.Any(p => p.Title.ToLower() == model.Title.ToLower()));

            if (isProjectSubTicketExist)
            {
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectSubTicketAlreadyExists,
                    $"Project sub-ticket with project ticket ID {model.ProjectTicketId} and title {model.Title} already exists.");
            }

            ProjectSubTiket projectSubTiket = new(model.ProjectTicketId, model.Title);

            List<ProjectSubTiketUser> projectSubTiketUsers = new();
            foreach (var projectTicketUserId in model.projectTicketUserIds!)
            {
                var projectTicketUserExists = await _agileDbContext.ProjectTiketUsers.AnyAsync(pu => pu.Id == projectTicketUserId);
                if (!projectTicketUserExists)
                {
                    throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketUserNotFound,
                        $"Project ticket user with ID {projectTicketUserId} doesn't exist.");
                }

                ProjectSubTiketUser user = new() { ProjectSubTiketId = projectSubTiket.Id, ProjectTiketUserId = projectTicketUserId };
                projectSubTiketUsers.Add(user);
            }

            await _agileDbContext.ProjectSubTikets.AddAsync(projectSubTiket);
            await _agileDbContext.ProjectSubTiketUsers.AddRangeAsync(projectSubTiketUsers);
            await _agileDbContext.SaveChangesAsync();

            return new Response<ProjectSubTiket>("Successfully added!", projectSubTiket);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<ProjectSubTiket>> UpdateProjectSubTicketAsync(UpdateProjectSubTiketModel modelUpdate)
    {
        try
        {
            var isProjectSubTicketExist = await _agileDbContext.ProjectTikets
                .AnyAsync(p => p.Id == modelUpdate.ProjectSubTicketId
                               && p.ProjectSubTikets!.Any(p => p.Title.ToLower() == modelUpdate.Title.ToLower()));

            if (isProjectSubTicketExist)
            {
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectSubTicketAlreadyExists,
                    $"Project sub-ticket with ID {modelUpdate.ProjectSubTicketId} and title {modelUpdate.Title} already exists.");
            }

            await _agileDbContext.ProjectSubTiketUsers
                .Where(ptu => ptu.ProjectSubTiketId == modelUpdate.Id)
                .ExecuteDeleteAsync();

            List<ProjectSubTiketUser> projectSubTiketUsers = new();
            foreach (var projectTicketUserId in modelUpdate.projectTicketUserIds!)
            {
                var projectTicketUserExists = await _agileDbContext.ProjectTiketUsers.AnyAsync(pu => pu.Id == projectTicketUserId);
                if (!projectTicketUserExists)
                {
                    throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketUserNotFound,
                        $"Project ticket user with ID {projectTicketUserId} doesn't exist in this ticket.");
                }

                ProjectSubTiketUser user = new() { ProjectTiketUserId = projectTicketUserId, ProjectSubTiketId = modelUpdate.Id };
                projectSubTiketUsers.Add(user);
            }

            await _agileDbContext.ProjectSubTikets
                .Where(pt => pt.Id == modelUpdate.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(u => u.Deadline, modelUpdate.Deadline)
                    .SetProperty(u => u.Title, modelUpdate.Title));

            await _agileDbContext.ProjectSubTiketUsers.AddRangeAsync(projectSubTiketUsers);
            await _agileDbContext.SaveChangesAsync();

            return new Response<ProjectSubTiket>("Successfully updated!", new ProjectSubTiket());
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<Response<ProjectSubTiket>> DeleteProjectSubTicketAsync(Guid id)
    {
        try
        {
            var projectSubTicket = await _agileDbContext.ProjectSubTikets.FindAsync(id);
            if (projectSubTicket == null)
            {
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectSubTicketNotFound,
                    $"Project sub-ticket with ID {id} doesn't exist.");
            }

            await _agileDbContext.ProjectSubTiketUsers
                .Where(pt => pt.ProjectSubTiketId == id)
                .ExecuteDeleteAsync();

            await _agileDbContext.ProjectSubTikets
                .Where(pt => pt.Id == id)
                .ExecuteDeleteAsync();

            await _agileDbContext.SaveChangesAsync();
            return new Response<ProjectSubTiket>("Successfully deleted!", new ProjectSubTiket());
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<ProjectSubTiket?> GetProjectSubTicketAsync(Guid id)
    {
        try
        {
            var projectSubTicket = await _agileDbContext.ProjectSubTikets
                .Include(pst => pst.ProjectSubTiketUsers)
                    .ThenInclude(pst => pst.ProjectTiketUser)
                        .ThenInclude(pst => pst.ProjectUser)
                            .ThenInclude(pst => pst.Client)
                                .ThenInclude(pst => pst!.User)
                .Include(pst => pst.ProjectSubTiketUsers)
                    .ThenInclude(pst => pst.ProjectTiketUser)
                        .ThenInclude(pst => pst.ProjectUser)
                            .ThenInclude(pst => pst.Staff)
                                .ThenInclude(pst => pst!.User)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (projectSubTicket == null)
            {
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectSubTicketNotFound, $"Project sub-ticket with ID {id} not found.");
            }

            return projectSubTicket;
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<List<ProjectSubTiket>> GetProjectSubTicketsAsync()
    {
        try
        {
            var projectSubTickets = await _agileDbContext.ProjectSubTikets
                .Include(pst => pst.ProjectSubTiketUsers)
                    .ThenInclude(pst => pst.ProjectTiketUser)
                        .ThenInclude(pst => pst.ProjectUser)
                            .ThenInclude(pst => pst.Client)
                                .ThenInclude(pst => pst!.User)
                .Include(pst => pst.ProjectSubTiketUsers)
                    .ThenInclude(pst => pst.ProjectTiketUser)
                        .ThenInclude(pst => pst.ProjectUser)
                            .ThenInclude(pst => pst.Staff)
                                .ThenInclude(pst => pst!.User)
                .ToListAsync();

            if (projectSubTickets == null) return new List<ProjectSubTiket>();

            return projectSubTickets;
        }
        catch (Exception)
        {
            throw;
        }
    }
}