using PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.SubTickets;

namespace PersonalAccount.API.Services.Abstractions;

public interface IProjectSubTicketService
{
    Task<ProjectSubTiket?> GetProjectSubTicketAsync(Guid id);
    Task<List<ProjectSubTiket>> GetProjectSubTicketsAsync();
    Task<Response<ProjectSubTiket>> CreateProjectSubTicketAsync(ProjectSubTiketModel model);
    Task<Response<ProjectSubTiket>> UpdateProjectSubTicketAsync(UpdateProjectSubTiketModel modelUpdate);
    Task<Response<ProjectSubTiket>> DeleteProjectSubTicketAsync(Guid id);
}