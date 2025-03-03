using Application.Core;
using Application.Tasks.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tasks.Handlers;

public class Details
{
    public class Query : IRequest<Result<TaskDto>>
    {
        public Guid Id { get; set; }
    }


    public class Handler : IRequestHandler<Query, Result<TaskDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<TaskDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks
                .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            return Result<TaskDto>.Success(task);
        }
    }
}