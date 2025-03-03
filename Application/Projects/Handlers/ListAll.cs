using Application.Core;
using Application.Interfaces;
using Application.Projects.DTOs;
using Application.Tasks.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Projects.Handlers;

public class ListAll
{
    public class Query : IRequest<Result<List<ProjectDto>>>
    {
    }


    public class Handler : IRequestHandler<Query, Result<List<ProjectDto>>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IUserAccessor userAccessor, IMapper mapper)
        {
            _context = context;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }


        public async Task<Result<List<ProjectDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Projects
                .Where(p => p.Owner.DeviceId == _userAccessor.GetDeviceId())
                .OrderBy(p => p.CreateAt)
                .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var projects = await query.ToListAsync(cancellationToken);

            return Result<List<ProjectDto>>.Success(projects);
        }
    }
}