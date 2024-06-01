using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Application.Interfaces.Services;
using Application.Models;
using Application.Requests;
using Domain.Entities.Users;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Application.Features.Users.UserProfileImages.Commands
{
    public class UploadUserProfileImageCommand : UploadDataFileCommand, IRequest<Result>
    {
    }

    public class UploadUserProfileImageCommandHandler : IRequestHandler<UploadUserProfileImageCommand, Result>
    {
        private readonly IDataFileStorageService _dataFileStorageService;
        private readonly IUnitOfWork<Guid> _unitOfWork;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IUserProfileImageRepository _userProfileImageRepository;

        public UploadUserProfileImageCommandHandler(
            IUnitOfWork<Guid> unitOfWork,
            IUserProfileImageRepository userProfileImageRepository,
            IStringLocalizer<SharedResource> localizer,
            IDataFileStorageService dataFileStorageService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _dataFileStorageService = dataFileStorageService;
            _userProfileImageRepository = userProfileImageRepository;
        }

        public async Task<Result> Handle(UploadUserProfileImageCommand command, CancellationToken cancellationToken)
        {
            if (command.File is null) { return await Result.FailAsync("File required"); }

            var user = await _unitOfWork.Repository<User>().GetByIdAsync(command.RefId, cancellationToken);
            if (user is null) { return await Result.FailAsync(_localizer["Not Found"]); }

            var userProfileImage = await _userProfileImageRepository.GetByUserIdFirstOrDefaultAsync(command.RefId);
            if (userProfileImage is not null) { return await Result.FailAsync("User has profile image"); }
            
            var uploadDataFileResult = (await _dataFileStorageService.UploadDataFileAsync<UploadUserProfileImageCommand>(command, cancellationToken));

            userProfileImage = new UserProfileImage
            {
                Name = command.RefId.ToString() + "_" + command.File.FileName,
                ContentType = command.File.ContentType,
                Size = command.File.Length,
                RefId = command.RefId,
                ContainerName = uploadDataFileResult.Item2,
                Url = uploadDataFileResult.Item1.Messages[0]
            };

            await _unitOfWork.Repository<UserProfileImage>().AddAsync(userProfileImage, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            return await Result.SuccessAsync(uploadDataFileResult.Item1.Messages[0]);
        }
    }
}
