using PersonalAccount.API.Models.Dtos.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;

namespace PersonalAccount.API.Services.Abstractions;

public interface IProjectTaskService
{
    Task<Response<ProjectTask>> GetProjectTaskByIdAsync(Guid id);
    Task<Response<List<ProjectTask>>> GetProjectTasksByProjectIdAsync(Guid projectId);
    Task<Response<List<ProjectTask>>> GetProjectTasksByTicketIdAsync(Guid projectTicketId);
    Task<Response<List<ProjectTask>>> GetProjectTasksBySubTicketIdAsync(Guid projectSubTicketId);
    Task<Response<ProjectTask>> CreateProjectTaskOnTicketAsync(ProjectTicketTaskModel dto);
    Task<Response<ProjectTask>> CreateProjectTaskOnSubTicketAsync(ProjectSubTicketTaskModel dto);
    Task<Response<ProjectTask>> UpdateProjectTaskAsync(UpdateProjectTaskModel updateModel);
    Task<Response<ProjectTask>> DeleteProjectTaskAsync(Guid id);
    Task<Response<ProjectTask>> ChangeStateProjectTaskAsync(Guid id,string doneState);
}