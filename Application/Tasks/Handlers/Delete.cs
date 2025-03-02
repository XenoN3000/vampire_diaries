using Application.Core;
using MediatR;
using Persistence;

namespace Application.Tasks.Handlers;

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
            var task = await _context.Tasks.FindAsync(request.Id, cancellationToken);

            if (task is null) return null;

            _context.Tasks.Remove(task);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete Diary");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}