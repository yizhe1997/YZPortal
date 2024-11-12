using Application.Interfaces.Repositories;
using Application.Interfaces.Repositories.Users;
using Application.Interfaces.Services;
using Application.Models;
using Application.Requests;
using Domain.Entities.Users;
using FluentValidation;
using MediatR;

namespace Application.Features.Users.UserProfileImages.Commands
{
    public class UploadUserProfileImageCommand : UploadDataFileCommand, IRequest<Result>
    {
    }

    public class UpdatePortalConfigCommandValidator : AbstractValidator<UploadDataFileCommand>
    {
        public UpdatePortalConfigCommandValidator()
        {
            RuleFor(p => p.File)
            .NotNull();
            RuleFor(p => p.RefId)
            .NotNull();
        }
    }

    public class UploadUserProfileImageCommandHandler(
        IUnitOfWork<Guid> unitOfWork,
        IUserProfileImageRepository userProfileImageRepository,
        IDataFileStorageService dataFileStorageService) : IRequestHandler<UploadUserProfileImageCommand, Result>
    {
        public async Task<Result> Handle(UploadUserProfileImageCommand command, CancellationToken cancellationToken)
        {
            // TODO: let validator handle
            if (command.File is null) { return await Result.FailAsync("File required"); }

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
