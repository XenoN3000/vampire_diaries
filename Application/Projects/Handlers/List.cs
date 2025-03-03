using Application.Core;
using Application.Interfaces;
using Application.Projects.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Projects.Handlers;

public class List
{
    public class Query : IRequest<Result<PagedList<ProjectDto>>>
    {
        public ProjectParams Params { get; set; }
    }


    public class Handler : IRequestHandler<Query, Result<PagedList<ProjectDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;


        public Handler(DataContext context, IMapper mapper, IUserAccessor userAccessor)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }


        public async Task<Result<PagedList<ProjectDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Projects
                .Where(p => p.Owner.DeviceId == _userAccessor.GetDeviceId())
                .OrderBy(p => p.CreateAt)
                .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var projects = await PagedList<ProjectDto>.CreateAsync(query, request.Params.pageNumber, request.Params.PageSize, cancellationToken);

            return Result<PagedList<ProjectDto>>.Success(projects);
        }
    }
}