using Application.Core;
using Application.Interfaces;
using Application.Projects.DTOs;
using Application.Projects.Validators;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Projects.Handlers;

public class Create
{
    public class Command : IRequest<Result<Unit>>
    {
        public CreateProjectDto CreateProjectDto { get; set; }
    }


    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.CreateProjectDto).SetValidator(new CreateProjectValidator());
        }
    }



    public class Handler : IRequestHandler<Command, Result<Unit>>
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


        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.DeviceId == _userAccessor.GetDeviceId(), cancellationToken);
            
            if (user is null) return Result<Unit>.Failure("Failed To Create Project due to unknown User !!!");
            
            var project = _mapper.Map<Project>(request.CreateProjectDto);

            _context.Projects.Add(project);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;
            
            return !result ? Result<Unit>.Failure("Failed to Create Project DB Error") : Result<Unit>.Success(Unit.Value);
        }
    }
}