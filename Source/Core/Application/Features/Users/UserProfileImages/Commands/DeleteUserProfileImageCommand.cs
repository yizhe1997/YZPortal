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

    public class DeleteUserProfileImageCommandHandler(
        IUserProfileImageRepository userProfileImageRepository,
        IUnitOfWork<Guid> unitOfWork,
        IStringLocalizer<SharedResource> localizer,
        IDataFileStorageService dataFileStorageService) : IRequestHandler<DeleteUserProfileImageCommand, Result>
    {
        public async Task<Result> Handle(DeleteUserProfileImageCommand command, CancellationToken cancellationToken)
        {
            var userProfileImage = await userProfileImageRepository.GetByUserIdFirstOrDefaultAsync(command.UserId, cancellationToken);
            if (userProfileImage != null)
            {
                await dataFileStorageService.DeleteDataFileAsync(userProfileImage, cancellationToken);

                await unitOfWork.Repository<UserProfileImage>().DeleteAsync(userProfileImage, cancellationToken);
                await unitOfWork.Commit(cancellationToken);

                return await Result.SuccessAsync("Profile image deleted successfully.");
            }
            else
            {
                return await Result.FailAsync(localizer["Not Found"]);
            }
        }
    }
}
