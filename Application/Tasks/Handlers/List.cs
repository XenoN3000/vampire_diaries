using Application.Core;
using Application.Interfaces;
using Application.Tasks.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;

namespace Application.Tasks.Handlers;

public class List
{
    public class Query : IRequest<Result<PagedList<TaskDto>>>
    {
        public Guid ProjectId { get; set; }
        public TaskParams Params { get; set; }
    }


    public class Handler : IRequestHandler<Query, Result<PagedList<TaskDto>>>
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


        public async Task<Result<PagedList<TaskDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Tasks
                .Where(d => d.ProjectId == request.ProjectId)
                .OrderBy(d => d.StartTim)
                .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var diaries = await PagedList<TaskDto>.CreateAsync(query, request.Params.pageNumber, request.Params.PageSize, cancellationToken);

            return Result<PagedList<TaskDto>>.Success(diaries);
        }
    }
}