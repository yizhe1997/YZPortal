using Application.Interfaces.Repositories.Users;
using Application.Interfaces.Services;
using Application.Models;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Text.Json.Serialization;

namespace Application.Features.Users.UserProfileImages.Commands
{
    public class DownloadUserProfileImageCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
    }

    public class DownloadUserProfileImageCommandHandler : IRequestHandler<DownloadUserProfileImageCommand, Result>
    {
        private readonly IDataFileStorageService _dataFileStorageService;
        private readonly IUserProfileImageRepository _userProfileImageRepository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public DownloadUserProfileImageCommandHandler(
            IUserProfileImageRepository userProfileImageRepository,
            IStringLocalizer<SharedResource> localizer,
            IDataFileStorageService dataFileStorageService)
        {
            _userProfileImageRepository = userProfileImageRepository;
            _localizer = localizer;
            _dataFileStorageService = dataFileStorageService;
        }

        public async Task<Result> Handle(DownloadUserProfileImageCommand command, CancellationToken cancellationToken)
        {
            var userProfileImage = await _userProfileImageRepository.GetByUserIdFirstOrDefaultAsync(command.UserId);
            if (userProfileImage != null)
            {
                return await _dataFileStorageService.DownloadDataFileAsync(userProfileImage, cancellationToken);
            }
            else
            {
                return await Result.FailAsync(_localizer["Not Found"]);
            }
        }
    }
}
