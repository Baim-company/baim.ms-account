using Global.Infrastructure.Exceptions.PersonalAccount;
using Microsoft.EntityFrameworkCore;
using PersonalAccount.API.Data.DbContexts;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Enums;
using PersonalAccount.API.Services.Abstractions;

namespace PersonalAccount.API.Services.Implementations;

public class ProjectTaskService : IProjectTaskService
{
    private readonly AgileDbContext _dbContext;
    public ProjectTaskService(AgileDbContext agileDbContext)
    {
        _dbContext = agileDbContext;
    }


    public async Task<Response<ProjectTask>> ChangeStateProjectTaskAsync(Guid id, string doneState)
    {
        try
        {
            var projectTaskExist = await _dbContext.ProjectTasks.FindAsync(id);
            if (projectTaskExist == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, $"Task with id: {id} doesn't exist!");

            var stateResponse = GetDoneState(doneState);
            if (stateResponse.Item2 == null)
                throw new PersonalAccountException(PersonalAccountErrorType.InvalidTaskState, $"Done state: {doneState} doesn't exist!");

            await _dbContext.ProjectTasks
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.DoneState, stateResponse.Item1));
            await _dbContext.SaveChangesAsync();

            return new Response<ProjectTask>("Successfully updated!", new ProjectTask());
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Response<ProjectTask>> CreateProjectTaskOnSubTicketAsync(ProjectSubTicketTaskModel dto)
    {
        try
        {
            var projectExist = await _dbContext.Projects.FindAsync(dto.ProjectId);
            if (projectExist == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectNotFound, $"Project with id: {dto.ProjectId} doesn't exist!");

            var projectTicketExist = await _dbContext.ProjectTikets.FindAsync(dto.ProjectTicketId);
            if (projectTicketExist == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketNotFound, $"Project's ticket with id: {dto.ProjectTicketId} doesn't exist!");

            var projectSubTicketExist = await _dbContext.ProjectSubTikets.FindAsync(dto.ProjectSubTicketId);
            if (projectSubTicketExist == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectSubTicketNotFound, $"Project's sub-ticket with id: {dto.ProjectSubTicketId} doesn't exist!");

            var projectTask = new ProjectTask(dto.Title, dto.Description, dto.IsVeryImportant, dto.DeadLine)
            {
                ProjectId = dto.ProjectId,
                ProjectTicketId = dto.ProjectTicketId,
                ProjectSubTicketId = dto.ProjectSubTicketId
            };
            await _dbContext.ProjectTasks.AddAsync(projectTask);

            if (dto.SubTicketTaskParticipants == null)
                throw new PersonalAccountException(PersonalAccountErrorType.GeneralError, "Participants can't be null!");

            foreach (var participant in dto.SubTicketTaskParticipants)
            {
                var projcetSubTictetUser = await _dbContext.ProjectSubTiketUsers.FindAsync(participant.ProjectSubTiketUserId);
                if (projcetSubTictetUser == null)
                    throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketUserNotFound, $"Sub-ticket user with id: {participant.ProjectSubTiketUserId} doesn't exist!");

                var taskRoleResponse = GetTaskRole(participant.TaskRole);
                if (taskRoleResponse.Item2 == null)
                    throw new PersonalAccountException(PersonalAccountErrorType.TaskRoleNotFound, $"Task role: {participant.TaskRole} doesn't exist!");

                var projectSubTicketTaskUser = new ProjectSubTicketTaskUser(taskRoleResponse.Item1)
                {
                    ProjectSubTiketUserId = projcetSubTictetUser.Id,
                    ProjectTaskId = projectTask.Id
                };
                await _dbContext.ProjectSubTicketTaskUsers.AddAsync(projectSubTicketTaskUser);
            }

            await _dbContext.SaveChangesAsync();

            return new Response<ProjectTask>("Successfully created!", projectTask);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<Response<ProjectTask>> CreateProjectTaskOnTicketAsync(ProjectTicketTaskModel dto)
    {
        try
        {
            var projectExist = await _dbContext.Projects.FindAsync(dto.ProjectId);
            if (projectExist == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectNotFound, $"Project with id: {dto.ProjectId} doesn't exist!");

            var projectTicketExist = await _dbContext.ProjectTikets.FindAsync(dto.ProjectTicketId);
            if (projectTicketExist == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketNotFound, $"Project's ticket with id: {dto.ProjectTicketId} doesn't exist!");

            var projectTask = new ProjectTask(dto.Title, dto.Description, dto.IsVeryImportant, dto.DeadLine)
            {
                ProjectId = dto.ProjectId,
                ProjectTicketId = dto.ProjectTicketId
            };
            await _dbContext.ProjectTasks.AddAsync(projectTask);

            if (dto.TicketTaskParticipants == null)
                throw new PersonalAccountException(PersonalAccountErrorType.GeneralError, "Participants can't be null!");

            foreach (var participant in dto.TicketTaskParticipants)
            {
                var projcetTictetUser = await _dbContext.ProjectTiketUsers.FindAsync(participant.ProjectTiketUserId);
                if (projcetTictetUser == null)
                    throw new PersonalAccountException(PersonalAccountErrorType.ProjectTicketUserNotFound, $"Ticket user with id: {participant.ProjectTiketUserId} doesn't exist!");

                var taskRoleResponse = GetTaskRole(participant.TaskRole);
                if (taskRoleResponse.Item2 == null)
                    throw new PersonalAccountException(PersonalAccountErrorType.TaskRoleNotFound, $"Task role: {participant.TaskRole} doesn't exist!");

                var projectTicketTaskUser = new ProjectTicketTaskUser(taskRoleResponse.Item1)
                {
                    ProjectTiketUserId = projcetTictetUser.Id,
                    ProjectTaskId = projectTask.Id
                };
                await _dbContext.ProjectTicketTaskUsers.AddAsync(projectTicketTaskUser);
            }

            await _dbContext.SaveChangesAsync();

            return new Response<ProjectTask>("Successfully created!", projectTask);
        }
        catch (Exception)
        {
            throw ;
        }
    }


    public async Task<Response<ProjectTask>> DeleteProjectTaskAsync(Guid id)
    {
        var projectTask = await _dbContext.ProjectTasks.FirstOrDefaultAsync(pt => pt.Id == id);

        if (projectTask == null)
            return new Response<ProjectTask>("ProjectTask not found", null );

        _dbContext.ProjectTasks.Remove(projectTask);
        await _dbContext.SaveChangesAsync();

        return new Response<ProjectTask>("ProjectTask deleted successfully", projectTask );
    }

    public async Task<Response<ProjectTask>> GetProjectTaskByIdAsync(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                throw new PersonalAccountException(PersonalAccountErrorType.GeneralError, "Invalid task ID.");

            var projectTask = await _dbContext.ProjectTasks
                .AsNoTracking()
                .Include(pt => pt.ProjectTicketTaskUsers!)
                    .ThenInclude(ptu => ptu.ProjectTiketUser!)
                        .ThenInclude(pu => pu.ProjectUser!)
                            .ThenInclude(pu => pu.Client!)
                                .ThenInclude(c => c.User)
                .Include(pt => pt.ProjectTicketTaskUsers!)
                    .ThenInclude(ptu => ptu.ProjectTiketUser!)
                        .ThenInclude(pu => pu.ProjectUser!)
                            .ThenInclude(pu => pu.Staff!)
                                .ThenInclude(s => s.User)
                .Include(pt => pt.ProjectSubTicketTaskUsers!)
                    .ThenInclude(pstu => pstu.ProjectSubTiketUser!)
                        .ThenInclude(pstu => pstu.ProjectTiketUser!)
                            .ThenInclude(ptu => ptu.ProjectUser!)
                                .ThenInclude(pu => pu.Client!)
                                    .ThenInclude(c => c.User)
                .Include(pt => pt.ProjectSubTicketTaskUsers!)
                    .ThenInclude(pstu => pstu.ProjectSubTiketUser!)
                        .ThenInclude(pstu => pstu.ProjectTiketUser!)
                            .ThenInclude(ptu => ptu.ProjectUser!)
                                .ThenInclude(pu => pu.Staff!)
                                    .ThenInclude(s => s.User)
                .Include(pt => pt.Project)
                .Include(pt => pt.ProjectTicket)
                .Include(pt => pt.ProjectSubTicket)
                .Include(pt => pt.ProjectTaskCheckList)
                .Include(pt => pt.Comments)
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (projectTask == null)
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "Task not found");

            return new Response<ProjectTask>("Task retrieved successfully", projectTask);
        }
        catch (Exception)
        {
            throw;
        }
    }



    public async Task<Response<List<ProjectTask>>> GetProjectTasksByProjectIdAsync(Guid projectId)
    {
        try
        {
            if (projectId == Guid.Empty)
                throw new PersonalAccountException(PersonalAccountErrorType.GeneralError, "Invalid project ID.");

            var projectTasks = await _dbContext.ProjectTasks
                .AsNoTracking()
                .Include(pt => pt.ProjectTicketTaskUsers!)
                    .ThenInclude(ptu => ptu.ProjectTiketUser!)
                        .ThenInclude(pu => pu.ProjectUser!)
                            .ThenInclude(pu => pu.Client!)
                                .ThenInclude(c => c.User)
                .Include(pt => pt.ProjectTicketTaskUsers!)
                    .ThenInclude(ptu => ptu.ProjectTiketUser!)
                        .ThenInclude(pu => pu.ProjectUser!)
                            .ThenInclude(pu => pu.Staff!)
                                .ThenInclude(s => s.User)
                .Include(pt => pt.ProjectSubTicketTaskUsers!)
                    .ThenInclude(pstu => pstu.ProjectSubTiketUser!)
                        .ThenInclude(pstu => pstu.ProjectTiketUser!)
                            .ThenInclude(ptu => ptu.ProjectUser!)
                                .ThenInclude(pu => pu.Client!)
                                    .ThenInclude(c => c.User)
                .Include(pt => pt.ProjectSubTicketTaskUsers!)
                    .ThenInclude(pstu => pstu.ProjectSubTiketUser!)
                        .ThenInclude(pstu => pstu.ProjectTiketUser!)
                            .ThenInclude(ptu => ptu.ProjectUser!)
                                .ThenInclude(pu => pu.Staff!)
                                    .ThenInclude(s => s.User)
                .Where(pt => pt.ProjectId == projectId)
                .ToListAsync();

            if (projectTasks == null || !projectTasks.Any())
                throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "No tasks found for the specified project");

            return new Response<List<ProjectTask>>("Tasks retrieved successfully", projectTasks);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<Response<List<ProjectTask>>> GetProjectTasksBySubTicketIdAsync(Guid projectSubTicketId)
    {
        try
        {
            if (projectSubTicketId == Guid.Empty)
            {
                return new Response<List<ProjectTask>>("Invalid ID.", null);
            }

            var projectTasks = await _dbContext.ProjectTasks
                .AsNoTracking()
                .Include(pt => pt.ProjectTicketTaskUsers!)
                    .ThenInclude(ptu => ptu.ProjectTiketUser!)
                        .ThenInclude(pu => pu.ProjectUser!)
                            .ThenInclude(pu => pu.Client!)
                                .ThenInclude(c => c.User)
                .Include(pt => pt.ProjectTicketTaskUsers!)
                    .ThenInclude(ptu => ptu.ProjectTiketUser!)
                        .ThenInclude(pu => pu.ProjectUser!)
                            .ThenInclude(pu => pu.Staff!)
                                .ThenInclude(s => s.User)
                .Include(pt => pt.ProjectSubTicketTaskUsers!)
                    .ThenInclude(pstu => pstu.ProjectSubTiketUser!)
                        .ThenInclude(pstu => pstu.ProjectTiketUser!)
                            .ThenInclude(ptu => ptu.ProjectUser!)
                                .ThenInclude(pu => pu.Client!)
                                    .ThenInclude(c => c.User)
                .Include(pt => pt.ProjectSubTicketTaskUsers!)
                    .ThenInclude(pstu => pstu.ProjectSubTiketUser!)
                        .ThenInclude(pstu => pstu.ProjectTiketUser!)
                            .ThenInclude(ptu => ptu.ProjectUser!)
                                .ThenInclude(pu => pu.Staff!)
                                    .ThenInclude(s => s.User)
                .Where(pt => pt.ProjectSubTicketId == projectSubTicketId)
                .ToListAsync();

            if (projectTasks == null || !projectTasks.Any())
            {
                return new Response<List<ProjectTask>>("No tasks found for the specified sub-ticket.", null);
            }

            return new Response<List<ProjectTask>>("Tasks retrieved successfully", projectTasks);
        }
        catch (Exception ex)
        {
            return new Response<List<ProjectTask>>($"An error occurred while retrieving tasks: {ex.Message}", null);
        }
    }

    public async Task<Response<List<ProjectTask>>> GetProjectTasksByTicketIdAsync(Guid projectTicketId)
    {
        try
        {
            if (projectTicketId == Guid.Empty) 
                throw new PersonalAccountException(PersonalAccountErrorType.GeneralError, "Invalid ticket ID.");

            var projectTasks = await _dbContext.ProjectTasks
                .AsNoTracking()
                .Include(pt => pt.ProjectTicketTaskUsers!)
                    .ThenInclude(ptu => ptu.ProjectTiketUser!)
                        .ThenInclude(pu => pu.ProjectUser!)
                            .ThenInclude(pu => pu.Client!)
                                .ThenInclude(c => c.User)
                .Include(pt => pt.ProjectTicketTaskUsers!)
                    .ThenInclude(ptu => ptu.ProjectTiketUser!)
                        .ThenInclude(pu => pu.ProjectUser!)
                            .ThenInclude(pu => pu.Staff!)
                                .ThenInclude(s => s.User)
                .Include(pt => pt.ProjectSubTicketTaskUsers!)
                    .ThenInclude(pstu => pstu.ProjectSubTiketUser!)
                        .ThenInclude(pstu => pstu.ProjectTiketUser!)
                            .ThenInclude(ptu => ptu.ProjectUser!)
                                .ThenInclude(pu => pu.Client!)
                                    .ThenInclude(c => c.User)
                .Include(pt => pt.ProjectSubTicketTaskUsers!)
                    .ThenInclude(pstu => pstu.ProjectSubTiketUser!)
                        .ThenInclude(pstu => pstu.ProjectTiketUser!)
                            .ThenInclude(ptu => ptu.ProjectUser!)
                                .ThenInclude(pu => pu.Staff!)
                                    .ThenInclude(s => s.User)
                .Where(pt => pt.ProjectTicketId == projectTicketId)
                .ToListAsync();

            if (projectTasks == null || !projectTasks.Any()) throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "No tasks found for the specified ticket.");

            return new Response<List<ProjectTask>>("Tasks retrieved successfully", projectTasks);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Response<ProjectTask>> UpdateProjectTaskAsync(UpdateProjectTaskModel updateModel)
    {
        try
        {
            if (updateModel.Id == Guid.Empty) 
                throw new PersonalAccountException(PersonalAccountErrorType.GeneralError, "Invalid task ID.");

            var existingTask = await _dbContext.ProjectTasks
                .AsTracking()
                .FirstOrDefaultAsync(pt => pt.Id == updateModel.Id);

            if (existingTask == null) throw new PersonalAccountException(PersonalAccountErrorType.ProjectTaskNotFound, "Task not found.");

            bool isUpdated = false;

            if (existingTask.Title != updateModel.Title)
            {
                existingTask.Title = updateModel.Title;
                isUpdated = true;
            }

            if (existingTask.Description != updateModel.Description)
            {
                existingTask.Description = updateModel.Description;
                isUpdated = true;
            }

            if (existingTask.IsVeryImportant != updateModel.IsVeryImportant)
            {
                existingTask.IsVeryImportant = updateModel.IsVeryImportant;
                isUpdated = true;
            }

            if (existingTask.DeadLine != updateModel.DeadLine)
            {
                existingTask.DeadLine = updateModel.DeadLine;
                isUpdated = true;
            }

            if (!isUpdated) 
                return new Response<ProjectTask>("No changes detected.", existingTask);

            _dbContext.ProjectTasks.Update(existingTask);
            await _dbContext.SaveChangesAsync();

            return new Response<ProjectTask>("Task updated successfully.", existingTask);
        }
        catch (Exception)
        {
            throw;
        }
    }
    private (DoneState,string?) GetDoneState(string doneState)
    {
        doneState = doneState.Trim().ToLower();
        return doneState switch
        {
            "done" => (DoneState.Done,"Success!"),
            "cancelled" => (DoneState.Cancelled,"Success!"),
            "inprocess" => (DoneState.InProcess,"Success!"),
            "pending" => (DoneState.Pending,"Success!"),
            _ => (DoneState.Cancelled, null)
        };
    }
    private (ProjectUserRole, string?) GetTaskRole(string taskRole)
    {
        taskRole = taskRole.Trim().ToLower();
        return taskRole switch
        {
            "executor" => (ProjectUserRole.Executor,"Success!"),
            "observer" => (ProjectUserRole.Observer,"Success!"),
            "director" => (ProjectUserRole.Director,"Success!"),
            "coexecutor" => (ProjectUserRole.CoExecutor,"Success!"),
            _ => (ProjectUserRole.Observer, null)
        };
    }
}