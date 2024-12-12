using PersonalAccount.API.Models.Dtos.Agiles.CheckItems;
using PersonalAccount.API.Models.Dtos.Responses;
using System;
using System.Threading.Tasks;

namespace PersonalAccount.API.Services
{
    public interface IProjectTaskCheckListService
    {
        Task<Response<ProjectTaskCheckListModel>> CreateCheckListAsync(ProjectTaskCheckListCreateModel checkListCreateModel);
        Task<Response<ProjectTaskCheckListModel>> UpdateCheckListAsync(ProjectTaskCheckListModel checkListModel);
        Task<Response<ProjectTaskCheckListModel>> GetCheckListByTaskIdAsync(Guid projectTaskId);
        Task<Response<bool>> DeleteCheckListAsync(Guid id);
    }

}
