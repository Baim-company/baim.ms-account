using PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;

namespace PersonalAccount.API.Services.Abstractions;

public interface IProjectTicketService
{
    Task<ProjectTiket?> GetProjectTicketByIdAsync(Guid id);
    Task<List<ProjectTiket>?> GetProjectTicketsAsync();
    Task<Response<ProjectTiket>> CreateProjectTicketAsync(ProjectTiketModel projectTiket);
    Task<Response<ProjectTiket>> UpdateProjectTicketAsync(UpdateProjectTiketModel updateProjectTiket);
    Task<Response<ProjectTiket>> DeleteProjectTicketAsync(Guid id);
}