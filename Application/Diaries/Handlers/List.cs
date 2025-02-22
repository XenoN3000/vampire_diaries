using Application.Core;
using Application.Diaries.DTOs;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Persistence;

namespace Application.Diaries.Handlers;

public class List
{
    public class Query : IRequest<Result<PagedList<DiaryDto>>>
    {
        public DiaryParams Params { get; set; }
    }


    public class Handler : IRequestHandler<Query, Result<PagedList<DiaryDto>>>
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


        public async Task<Result<PagedList<DiaryDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _context.Diaries
                .Where(d => d.OwnerId == _userAccessor.GetDeviceId())
                .OrderBy(d => d.StartTim)
                .ProjectTo<DiaryDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            var diaries = await PagedList<DiaryDto>.CreateAsync(query, request.Params.pageNumber, request.Params.PageSize, cancellationToken);

            return Result<PagedList<DiaryDto>>.Success(diaries);
        }
    }
}