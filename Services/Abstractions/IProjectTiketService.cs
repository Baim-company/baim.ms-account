using PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;

namespace PersonalAccount.API.Services.Abstractions;
public interface IProjectTiketService
{
    public Task<List<ProjectTiket>> GetProjectTiketsAsync();
    public Task<Response<ProjectTiket>> GetProjectTiketAsync(Guid id);

    public Task<Response<ProjectTiket>> AddProjectTiketAsync(ProjectTiketModel projectTiketModel);
    public Task<Response<ProjectTiket>> UpdateProjectTiketAsync(UpdateProjectTiketModel projectTiketModel);

    public Task<Response<ProjectTiket>> DeleteProjectTiketAsync(Guid id);
}