using Application.Core;
using Application.Diaries.DTOs;
using Application.Diaries.Vlidators;
using AutoMapper;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Diaries.Handlers;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public DiaryDto DiaryDto { get; set; }
    }



    public class CommandValidator : AbstractValidator<Create.Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.DiaryDto).SetValidator(new DiaryValidator());
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
            var diary = await _context.Diaries.FindAsync(request.DiaryDto.Id, cancellationToken);

            if (diary is null) return null;

            _mapper.Map(request.DiaryDto, diary);
            
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            return !result ? Result<Unit>.Failure("Failed to update Diary") : Result<Unit>.Success(Unit.Value);

        }
    }
}