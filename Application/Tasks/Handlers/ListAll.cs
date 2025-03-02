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
    }


    public class Handler : IRequestHandler<Query, Result<List<TaskDto>>>
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


        public async Task<Result<List<TaskDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Tasks
                .Where(d => d.OwnerId == _userAccessor.GetDeviceId())
                .OrderBy(d => d.StartTim)
                .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var diaries = await query.ToListAsync(cancellationToken);

            return Result<List<TaskDto>>.Success(diaries);
        }
    }
}