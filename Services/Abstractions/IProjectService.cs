using PersonalAccount.API.Models.Dtos;
using PersonalAccount.API.Models.Dtos.Agiles.Projects;
using PersonalAccount.API.Models.Dtos.Responses;
using PersonalAccount.API.Models.Entities.Agiles.Projects;

namespace PersonalAccount.API.Services.Abstractions;
public interface IProjectService
{
    public Task<Response<Project>> GetProjectAsync(Guid id);
    public Task<PagedResponse<Project>> GetProjcetsAsync(PaginationParameters paginationParameters ,string? onFilter ,string? onProduct = null, bool sortByCreationDate = false);
    public Task<PagedResponse<Project>> GetProjcetsByCompanyAsync(Guid companyId,PaginationParameters paginationParameters ,string? onFilter ,string? onProduct = null, bool sortByCreationDate = false);
    public Task<PagedResponse<Project>> GetProjcetsByParticipationAsync(Guid participationId, PaginationParameters paginationParameters ,string? onFilter ,string? onProduct = null, bool sortByCreationDate = false);

    public Task<Response<Project>> AddProjectAsync(ProjectModel projectModel);
    public Task<Response<Project>> UpdateProjectAsync(UpdateProjectModel projectModel); 
    public Task<Response<Project>> CompleteProjectAsync(Guid id); 
    public Task<Response<Project>> MakeProjectPublicAsync(Guid id); 
    public Task<Response<Project>> DeleteProjectAsync(Guid id); 
}