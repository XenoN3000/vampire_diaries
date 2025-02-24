using Application.Core;
using Application.Diaries.DTOs;
using Application.Diaries.Vlidators;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Diaries.Handlers;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public CreateDiaryDto DiaryDto { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.DiaryDto).SetValidator(new DiaryValidator());
        }
    }


    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IUserAccessor _userAccessor;

        public Handler(DataContext context, IUserAccessor userAccessor)
        {
            _context = context;
            _userAccessor = userAccessor;
        }


        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.DeviceId == _userAccessor.GetDeviceId(), cancellationToken);

            if (user is null) return Result<Unit>.Failure("Failed To Create Diary due to unknown user !!!");

            var diary = new Diary()
            {
                Id = request.DiaryDto.Id,
                Title = request.DiaryDto.Title,
                Description = request.DiaryDto.Description,
                StartTim = request.DiaryDto.Date,
                Duration = request.DiaryDto.Duration,
                OwnerId = user.DeviceId,
                Owner = user
            };

            _context.Diaries.Add(diary);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) Result<Unit>.Failure("Failed to create Diary DB error ");
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}