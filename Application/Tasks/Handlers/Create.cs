using Application.Core;
using Application.Diaries.Vlidators;
using Application.Interfaces;
using Application.Tasks.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Tasks.Handlers;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public CreateTaskDto TaskDto { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.TaskDto).SetValidator(new TaskValidator());
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

            var task = new Domain.Task()
            {
                Id = request.TaskDto.Id,
                Title = request.TaskDto.Title,
                Description = request.TaskDto.Description,
                StartTim = request.TaskDto.Date,
                Duration = request.TaskDto.Duration,
                OwnerId = user.DeviceId,
                Owner = user
            };

            _context.Tasks.Add(task);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) Result<Unit>.Failure("Failed to create Diary DB error ");
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}