using Application.Projects.DTOs;
using Application.Tasks.DTOs;
using AutoMapper;
using Domain;
using UserProfile = Application.Profiles.DTOs.Profile;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string currentDeviceId = null;

        CreateMap<Domain.Task, Domain.Task>();
        CreateMap<Domain.Task, TaskDto>()
            .ForMember(d => d.Date, o => o.MapFrom(s => s.StartTim));

        CreateMap<TaskDto, Domain.Task>()
            .ForMember(d => d.StartTim, o => o.MapFrom(s => s.Date))
            .ForMember(d => d.ProjectId, o => o.MapFrom(s => s.ProjectId));

        CreateMap<CreateTaskDto, Domain.Task>()
            .ForMember(d => d.StartTim, o => o.MapFrom(s => s.Date));


        CreateMap<UserProfile, UserProfile>();
        CreateMap<AppUser, UserProfile>()
            .ForMember(d => d.DeviceId, o => o.MapFrom(s => s.DeviceId))
            .ForMember(d => d.LastLogin,
                o => o.MapFrom(s => s.UserLogIns
                    .Where(u => u.User.DeviceId == currentDeviceId)
                    .Select(c => (DateTime?) c.LoggedInAt)
                    .DefaultIfEmpty()
                    .Max()))
            .ForAllMembers(opts => opts.Condition((s, d, sm) => sm != null));

        CreateMap<Project, Project>();
        CreateMap<Project, ProjectDto>();
        CreateMap<ProjectDto, Project>()
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.Owner.Id));
    }
}