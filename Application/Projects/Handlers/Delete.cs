using Application.Core;
using MediatR;
using Persistence;

namespace Application.Projects.Handlers;

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
            var project = await _context.Projects.FindAsync(request.Id, cancellationToken);

            if (project is null) return null;

            _context.Projects.Remove(project);
            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete Project");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}