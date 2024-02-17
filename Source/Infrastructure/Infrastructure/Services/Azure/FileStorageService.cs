using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Models;
using Application.Models.File;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using DomainEntity = Domain.Entities.Misc;
using Domain.Enums;
using Domain.Entities.Users;
using Application.Requests;

namespace Infrastructure.Services.Azure
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWork<Guid> _unitOfWork;
        private readonly BlobServiceClient _blobClient;
        private readonly AzureStorageConfig _azureStorageConfig;

        public FileStorageService(IUnitOfWork<Guid> unitOfWork, BlobServiceClient blobClient, IOptions<AzureStorageConfig> azureStorageConfig, IFileRepository fileRepository)
        {
            _blobClient = blobClient;
            _azureStorageConfig = azureStorageConfig.Value;
            _fileRepository = fileRepository;
            _unitOfWork = unitOfWork;
        }
        
        public async Task<Result> DeleteFileAsync(Guid id, CancellationToken cancellationToken)
        {
            var file = await _unitOfWork.Repository<DomainEntity.File>().GetByIdAsync(id, cancellationToken);

            if (file != null)
            {
                await _unitOfWork.Repository<DomainEntity.File>().DeleteAsync(file, cancellationToken);

                await _unitOfWork.Commit(cancellationToken);

                var blob = await ReferenceBlob(file.ContainerName, file.Name);

                if (blob != null)
                {
                    await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);

                    return await Result.SuccessAsync();
                }
            }

            return await Result.FailAsync("File not found");
        }

        public async Task<Result<DownloadFileModel>> DownloadFileAsync(Guid id, CancellationToken cancellationToken)
        {
            var file = await _unitOfWork.Repository<DomainEntity.File>().GetByIdAsync(id, cancellationToken);

            if (file != null)
            {
                var blob = await ReferenceBlob(file.ContainerName, file.Name);

                if (blob != null)
                {
                    var stream = new MemoryStream();

                    await blob.DownloadToAsync(stream, cancellationToken);

                    stream.Seek(0, SeekOrigin.Begin);

                    var result = new DownloadFileModel()
                    {
                        Stream = stream,
                        ContentType = file.ContentType,
                        FileName = file.Name
                    };

                    return await Result<DownloadFileModel>.SuccessAsync(result);
                }
            }

            return await Result<DownloadFileModel>.FailAsync("Fail to download");
        }

        public async Task<Result> UploadFileAsync(UploadFileCommand command, CancellationToken cancellationToken)
        {
            var containerName = "";
            var isValidRefId = false;

            switch (command.FileType)
            {
                case FileType.UserProfileImage:
                    {
                        isValidRefId = (await _unitOfWork.Repository<User>().GetByIdAsync(command.RefId, cancellationToken)) != null;
                        containerName = _azureStorageConfig.UserProfileImageContainer;
                        break;
                    }
                default: break;
            }

            if (!isValidRefId)
                return await Result.FailAsync("Invalid reference Id");

            if (string.IsNullOrEmpty(containerName))
                return await Result.FailAsync("Invalid container name");

            using var ms = new MemoryStream();

            command.File.CopyTo(ms);
            
            var filePoco = new DomainEntity.File
            {
                ContainerName = containerName,
                Name = command.FileName,
                ContentType = command.File.ContentType,
                RefType = (int)command.FileType,
                Size = command.File.Length,
                RefId = command.RefId
            };

            await _unitOfWork.Repository<DomainEntity.File>().AddAsync(filePoco, cancellationToken);
            await _unitOfWork.Commit(cancellationToken);

            var blob = await ReferenceBlob(filePoco.ContainerName, filePoco.Name, true);

            if (blob == null)
            {
                return await Result.FailAsync("Failed to reference blob");
            }

            ms.Seek(0, SeekOrigin.Begin);
            await blob.UploadAsync(ms, cancellationToken);

            return await Result.SuccessAsync();
        }

        #region Helpers

        /// <summary>
		///     Access the blob given the container and object name. Returns null if exception caught
		/// </summary>
		private async Task<BlobClient?> ReferenceBlob(string containerName, string fileName, bool toUpload = false)
        {
            // Container reference
            var containerClient = _blobClient.GetBlobContainerClient(containerName);

            if (!await containerClient.ExistsAsync())
                return null;

            var blobClient = containerClient.GetBlobClient(fileName);
            var blobExists = await blobClient.ExistsAsync();

            if (toUpload)
            {
                if (!blobExists)
                {
                    var uri = blobClient.GenerateSasUri(BlobSasPermissions.Write | BlobSasPermissions.Create, DateTime.MaxValue);
                    return new BlobClient(uri);
                }
                // UploadAsync will throw exception if blob exist already so no need to handle
            }

            return blobExists ? blobClient : null;
        }

        #endregion
    }
}
