using AutoMapper;
using PersonalAccount.API.Models.Dtos.Agiles.CheckItems;
using PersonalAccount.API.Models.Dtos.Agiles.Projects;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectSubTickets;
using PersonalAccount.API.Models.Dtos.Agiles.ProjectTickets;
using PersonalAccount.API.Models.Dtos.Users;
using PersonalAccount.API.Models.Entities.Agiles.CheckItems;
using PersonalAccount.API.Models.Entities.Agiles.CheckLists;
using PersonalAccount.API.Models.Entities.Agiles.Projects;
using PersonalAccount.API.Models.Entities.Agiles.ProjectTasks;
using PersonalAccount.API.Models.Entities.Agiles.SubTickets;
using PersonalAccount.API.Models.Entities.Agiles.Tickets;
using PersonalAccount.API.Models.Entities.Users;

namespace PersonalAccount.API.Services.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProjectTaskCheckList, ProjectTaskCheckListModel>()
            .ForMember(dest => dest.ProjectTaskId, opt => opt.MapFrom(src => src.ProjectTaskId)) 
            .ReverseMap();
        CreateMap<ProjectTaskCheckList, ProjectTaskCheckListCreateModel>()
            .ForMember(dest => dest.ProjectTaskId, opt => opt.MapFrom(src => src.ProjectTaskId)) 
            .ReverseMap();
        CreateMap<ProjectTaskCheckItem, ProjectTaskCheckItemModel>()
            .ReverseMap();
        CreateMap<ProjectTaskCheckItemFile, ProjectTaskCheckItemFileModel>()
            .ReverseMap();
    }
}
