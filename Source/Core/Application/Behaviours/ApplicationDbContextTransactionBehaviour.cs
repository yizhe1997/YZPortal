using Application.Interfaces.Contexts;
using MediatR;

namespace Application.Behaviours
{
    public class ApplicationDbContextTransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IApplicationDbContext _context;

        public ApplicationDbContextTransactionBehaviour(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse result;

            try
            {
                await _context.BeginTransactionAsync(cancellationToken);

                result = await next();

                await _context.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _context.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return result;
        }
    }
}
