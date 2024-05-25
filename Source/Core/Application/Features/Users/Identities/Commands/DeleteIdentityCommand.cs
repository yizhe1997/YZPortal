using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Application.Models;
using Domain.Entities.Users;
using MediatR;

namespace Application.Features.Users.UserProfileImages.Commands
{
    public class DeleteIdentityCommand : IRequest<Result>
    {
        public string? UserSubId { get; set; }
    }

    public class DeleteIdentityCommandCommandHandler : IRequestHandler<DeleteIdentityCommand, Result>
    {
        private readonly IUnitOfWork<Guid> _unitOfWork;
        private readonly IIdentityRepository _identityRepository;

        public DeleteIdentityCommandCommandHandler(
            IIdentityRepository identityRepository,
            IUnitOfWork<Guid> unitOfWork)
        {
            _identityRepository = identityRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteIdentityCommand command, CancellationToken cancellationToken)
        {
            var userIdentities = await _identityRepository.GetByUserSubIdAsync(command.UserSubId);
            if (userIdentities.Any())
            {
                await _unitOfWork.Repository<Identity>().DeleteRangeAsync(userIdentities, cancellationToken);
                await _unitOfWork.Commit(cancellationToken);

                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailAsync();
            }
        }
    }
}
