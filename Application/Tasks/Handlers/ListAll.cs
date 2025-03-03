using Application.Core;
using Application.Interfaces;
using Application.Tasks.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tasks.Handlers;

public class ListAll
{
    public class Query : IRequest<Result<List<TaskDto>>>
    {
        public Guid ProjectId { get; set; }
    }


    public class Handler : IRequestHandler<Query, Result<List<TaskDto>>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<List<TaskDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Tasks
                .Where(d => d.ProjectId == request.ProjectId)
                .OrderBy(d => d.StartTim)
                .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var tasks = await query.ToListAsync(cancellationToken);

            return Result<List<TaskDto>>.Success(tasks);
        }
    }
}