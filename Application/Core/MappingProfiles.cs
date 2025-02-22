using Application.Diaries.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    protected MappingProfiles()
    {
        CreateMap<Diary, Diary>();
        CreateMap<Diary, DiaryDto>()
            .ForMember(d => d.Date, o => o.MapFrom(s => s.StartTim));

        CreateMap<DiaryDto, Diary>()
            .ForMember(d => d.StartTim, o => o.MapFrom(s => s.Date))
            .ForMember(d => d.OwnerId, o => o.MapFrom(s => s.Owner.DeviceId));
    }
}