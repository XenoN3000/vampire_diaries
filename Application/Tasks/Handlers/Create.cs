using Application.Core;
using Application.Interfaces;
using Application.Tasks.DTOs;
using Application.Tasks.Validators;
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
            RuleFor(x => x.TaskDto).SetValidator(new CreateTaskValidator());
        }
    }


    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        public Handler(DataContext context)
        {
            _context = context;
        }


        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == request.TaskDto.ProjectId , cancellationToken);

            if (project is null) return Result<Unit>.Failure("Failed To Create Task due to unknown project !!!");

            var task = new Domain.Task()
            {
                Id = request.TaskDto.Id,
                Title = request.TaskDto.Title,
                Description = request.TaskDto.Description,
                StartTim = request.TaskDto.Date,
                Duration = request.TaskDto.Duration,
                ProjectId = request.TaskDto.ProjectId,
                Project = project
            };

            _context.Tasks.Add(task);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) Result<Unit>.Failure("Failed to create Diary DB error ");
            
            return Result<Unit>.Success(Unit.Value);
        }
    }
}