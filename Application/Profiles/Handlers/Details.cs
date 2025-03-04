using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using UserProfile = Application.Profiles.DTOs.Profile;

namespace Application.Profiles.Handlers;

public class Details
{

    public class Query : IRequest<Result<UserProfile>>
    {
    }




    public class Handler : IRequestHandler<Query, Result<UserProfile>>
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
        
        public async Task<Result<UserProfile>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .ProjectTo<UserProfile>(_mapper.ConfigurationProvider, new { currentDeviceId = _userAccessor.GetDeviceId() })
                .SingleOrDefaultAsync(x => x.DeviceId == _userAccessor.GetDeviceId(), cancellationToken);

            return Result<UserProfile>.Success(user);
        }
    }
}