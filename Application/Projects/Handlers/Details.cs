using Application.Core;
using Application.Projects.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Projects.Handlers;

public class Details
{
    public class Query : IRequest<Result<ProjectDto>>
    {
        public Guid Id { get; set; }
    }




    public class Handler : IRequestHandler<Query, Result<ProjectDto>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<ProjectDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            return Result<ProjectDto>.Success(project);
        }
    }
}