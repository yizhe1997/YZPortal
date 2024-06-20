using Application.Interfaces.Repositories.Users;
using Application.Interfaces.Services;
using Application.Models;
using Application.Models.File;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Text.Json.Serialization;

namespace Application.Features.Users.UserProfileImages.Commands
{
    public class DownloadUserProfileImageCommand : IRequest<Result<DownloadFileModel>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
    }

    public class DownloadUserProfileImageCommandHandler(
        IUserProfileImageRepository userProfileImageRepository,
        IStringLocalizer<SharedResource> localizer,
        IDataFileStorageService dataFileStorageService) : IRequestHandler<DownloadUserProfileImageCommand, Result<DownloadFileModel>>
    {
        public async Task<Result<DownloadFileModel>> Handle(DownloadUserProfileImageCommand command, CancellationToken cancellationToken)
        {
            var userProfileImage = await userProfileImageRepository.GetByUserIdFirstOrDefaultAsync(command.UserId, cancellationToken);
            if (userProfileImage != null)
            {
                return await dataFileStorageService.DownloadDataFileAsync(userProfileImage, cancellationToken);
            }
            else
            {
                return await Result<DownloadFileModel>.FailAsync(localizer["Not Found"]);
            }
        }
    }
}
