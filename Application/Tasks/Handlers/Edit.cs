using Application.Core;
using Application.Diaries.Vlidators;
using Application.Tasks.DTOs;
using AutoMapper;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Tasks.Handlers;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public TaskDto TaskDto { get; set; }
    }



    public class CommandValidator : AbstractValidator<Create.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.TaskDto).SetValidator(new TaskValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public Handler(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks.FindAsync(request.TaskDto.Id, cancellationToken);

            if (task is null) return null;

            _mapper.Map(request.TaskDto, task);
            
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return !result ? Result<Unit>.Failure("Failed to update Diary") : Result<Unit>.Success(Unit.Value);

        }
    }
}