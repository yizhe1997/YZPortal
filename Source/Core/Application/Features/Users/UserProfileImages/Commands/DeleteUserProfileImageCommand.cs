using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Application.Interfaces.Services;
using Application.Models;
using Domain.Entities.Users;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Text.Json.Serialization;

namespace Application.Features.Users.UserProfileImages.Commands
{
    public class DeleteUserProfileImageCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
    }

    public class DeleteUserProfileImageCommandHandler : IRequestHandler<DeleteUserProfileImageCommand, Result>
    {
        private readonly IDataFileStorageService _dataFileStorageService;
        private readonly IUnitOfWork<Guid> _unitOfWork;
        private readonly IUserProfileImageRepository _userProfileImageRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DeleteUserProfileImageCommandHandler(
            IUserProfileImageRepository userProfileImageRepository,
            IUnitOfWork<Guid> unitOfWork,
            IStringLocalizer<SharedResource> localizer,
            IDataFileStorageService dataFileStorageService)
        {
            _userProfileImageRepository = userProfileImageRepository;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _dataFileStorageService = dataFileStorageService;
        }

        public async Task<Result> Handle(DeleteUserProfileImageCommand command, CancellationToken cancellationToken)
        {
            var userProfileImage = await _userProfileImageRepository.GetByUserIdFirstOrDefaultAsync(command.UserId);
            if (userProfileImage != null)
            {
                await _dataFileStorageService.DeleteDataFileAsync(userProfileImage, cancellationToken);

                await _unitOfWork.Repository<UserProfileImage>().DeleteAsync(userProfileImage, cancellationToken);
                await _unitOfWork.Commit(cancellationToken);

                return await Result.SuccessAsync();
            }
            else
            {
                return await Result.FailAsync(_localizer["Not Found"]);
            }
        }
    }
}
