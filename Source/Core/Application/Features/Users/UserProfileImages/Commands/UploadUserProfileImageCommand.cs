using Application.Interfaces.Repositories;
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

        public UploadUserProfileImageCommandHandler(
            IUnitOfWork<Guid> unitOfWork,
            IStringLocalizer<SharedResource> localizer,
            IDataFileStorageService dataFileStorageService)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _dataFileStorageService = dataFileStorageService;
        }

        public async Task<Result> Handle(UploadUserProfileImageCommand command, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(command.RefId, cancellationToken);
            if (user != null && command.File != null)
            {
                var userProfileImage = new UserProfileImage
                {
                    Name = command.FileName,
                    ContentType = command.File.ContentType,
                    Size = command.File.Length,
                    RefId = command.RefId,
                    ContainerName = (await _dataFileStorageService.UploadDataFileAsync<UploadUserProfileImageCommand>(command, cancellationToken)).Item2
                };

                await _unitOfWork.Repository<UserProfileImage>().AddAsync(userProfileImage, cancellationToken);
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
