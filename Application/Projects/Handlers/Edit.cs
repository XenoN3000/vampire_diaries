using Application.Core;
using Application.Projects.DTOs;
using Application.Projects.Validators;
using AutoMapper;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Projects.Handlers;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public ProjectDto ProjectDto { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ProjectDto).SetValidator(new ProjectValidator());
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
            var project = await _context.Projects.FindAsync(request.ProjectDto.Id, cancellationToken);

            if (project is null) return null;

            _mapper.Map(request.ProjectDto, project);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            return !result ? Result<Unit>.Failure("Failed to update Project") : Result<Unit>.Success(Unit.Value);

        }
    }
}