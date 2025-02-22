using Application.Core;
using Application.Diaries.DTOs;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Diaries.Handlers;

public class ListAll
{
    public class Query : IRequest<Result<List<DiaryDto>>>
    {
    }


    public class Handler : IRequestHandler<Query, Result<List<DiaryDto>>>
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


        public async Task<Result<List<DiaryDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Diaries
                .Where(d => d.OwnerId == _userAccessor.GetDeviceId())
                .OrderBy(d => d.StartTim)
                .ProjectTo<DiaryDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var diaries = await query.ToListAsync(cancellationToken);

            return Result<List<DiaryDto>>.Success(diaries);
        }
    }
}