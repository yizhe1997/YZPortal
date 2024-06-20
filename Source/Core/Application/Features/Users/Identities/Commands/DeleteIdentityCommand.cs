using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Application.Models;
using Domain.Entities.Users;
using MediatR;

namespace Application.Features.Users.UserProfileImages.Commands
{
    //TODO: make it bulk delete
    public class DeleteIdentityCommand : IRequest<Result>
    {
        public string? UserSubId { get; set; }
    }

    public class DeleteIdentityCommandCommandHandler(
        IIdentityRepository identityRepository,
        IUnitOfWork<Guid> unitOfWork) : IRequestHandler<DeleteIdentityCommand, Result>
    {
        public async Task<Result> Handle(DeleteIdentityCommand command, CancellationToken cancellationToken)
        {
            var userIdentities = await identityRepository.GetByUserSubIdAsync(command.UserSubId, cancellationToken);
            if (userIdentities.Count != 0)
            {
                await unitOfWork.Repository<Identity>().DeleteRangeAsync(userIdentities, cancellationToken);
                await unitOfWork.Commit(cancellationToken);

                return await Result.SuccessAsync("Identity deleted successfully.");
            }
            else
            {
                return await Result.FailAsync();
            }
        }
    }
}
