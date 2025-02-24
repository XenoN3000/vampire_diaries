using Application.Core;
using MediatR;
using Persistence;

namespace Application.Diaries.Handlers;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
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
            var diary = await _context.Diaries.FindAsync(request.Id, cancellationToken);

            if (diary is null) return null;

            _context.Diaries.Remove(diary);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete Diary");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}