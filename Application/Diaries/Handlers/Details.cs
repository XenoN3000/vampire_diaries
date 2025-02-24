using Application.Core;
using Application.Diaries.DTOs;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Diaries.Handlers;

public class Details
{
    public class Query : IRequest<Result<DiaryDto>>
    {
        public Guid Id { get; set; }
    }


    public class Handler : IRequestHandler<Query, Result<DiaryDto>>
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


        public async Task<Result<DiaryDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var diary = await _context.Diaries
                .ProjectTo<DiaryDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

            return Result<DiaryDto>.Success(diary);
        }
    }
}