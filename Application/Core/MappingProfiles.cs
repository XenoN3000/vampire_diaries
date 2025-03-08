using Application.Tasks.DTOs;
using AutoMapper;
using Domain;
using UserProfile = Application.Profiles.DTOs.Profile;
using UserTask = Domain.Task;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string currentDeviceId = null;

        CreateMap<UserTask, UserTask>();
        CreateMap<UserTask, TaskDto>()
            .ForMember(d => d.Date, o => o.MapFrom(s => s.StartTim));

        CreateMap<TaskDto, UserTask>()
            .ForMember(d => d.StartTim, o => o.MapFrom(s => s.Date));
        CreateMap<CreateTaskDto, UserTask>()
            .ForMember(d => d.StartTim, o => o.MapFrom(s => s.Date));


        CreateMap<UserProfile, UserProfile>();
        CreateMap<AppUser, UserProfile>()
            .ForMember(d => d.DeviceId, o => o.MapFrom(s => s.DeviceId))
            .ForMember(d => d.LastLogin,
                o => o.MapFrom(s => s.UserLogIns
                    .Where(u => u.User.DeviceId == currentDeviceId)
                    .Select(c => c.LoggedInAt)
                    .DefaultIfEmpty()
                    .Max()))
            .ForAllMembers(opts => opts.Condition((s, d, sm) => sm != null));
    }
}