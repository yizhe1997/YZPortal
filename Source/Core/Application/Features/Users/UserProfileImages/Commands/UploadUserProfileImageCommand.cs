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

    public class UploadUserProfileImageCommandHandler(
        IUnitOfWork<Guid> unitOfWork,
        IUserProfileImageRepository userProfileImageRepository,
        IStringLocalizer<SharedResource> localizer,
        IDataFileStorageService dataFileStorageService) : IRequestHandler<UploadUserProfileImageCommand, Result>
    {
        public async Task<Result> Handle(UploadUserProfileImageCommand command, CancellationToken cancellationToken)
        {
            if (command.File is null) { return await Result.FailAsync("File required"); }

            var user = await unitOfWork.Repository<User>().GetByIdAsync(command.RefId, cancellationToken);
            if (user is null) { return await Result.FailAsync(localizer["Not Found"]); }

            var userProfileImage = await userProfileImageRepository.GetByUserIdFirstOrDefaultAsync(command.RefId, cancellationToken);
            if (userProfileImage is not null) { return await Result.FailAsync("User has profile image"); }
            
            var uploadDataFileResult = (await dataFileStorageService.UploadDataFileAsync<UploadUserProfileImageCommand>(command, cancellationToken));

            userProfileImage = new UserProfileImage
            {
                Name = command.RefId.ToString() + "_" + command.File.FileName,
                ContentType = command.File.ContentType,
                Size = command.File.Length,
                RefId = command.RefId,
                ContainerName = uploadDataFileResult.Item2,
                Url = uploadDataFileResult.Item1.Messages[0]
            };

            await unitOfWork.Repository<UserProfileImage>().AddAsync(userProfileImage, cancellationToken);
            await unitOfWork.Commit(cancellationToken);

            return await Result.SuccessAsync(uploadDataFileResult.Item1.Messages[0]);
        }
    }
}
