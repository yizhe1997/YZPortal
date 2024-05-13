using Application.Interfaces.Services;
using Application.Models;
using Application.Models.File;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Application.Requests;
using Domain.Entities.Misc;
using Application.Features.Users.UserProfileImages.Commands;
using Application.Interfaces.Repositories.Users;

namespace Infrastructure.Services.Storage
{
    public class DataFileStorageService : IDataFileStorageService
    {
        private readonly IUserProfileImageRepository _fileRepository;
        private readonly BlobServiceClient _blobClient;
        private readonly AzureStorageConfig _azureStorageConfig;

        public DataFileStorageService(BlobServiceClient blobClient, IOptions<AzureStorageConfig> azureStorageConfig, IUserProfileImageRepository fileRepository)
        {
            _blobClient = blobClient;
            _azureStorageConfig = azureStorageConfig.Value;
            _fileRepository = fileRepository;
        }

        public async Task<Result> DeleteDataFileAsync(DataFile dataFile, CancellationToken cancellationToken)
        {
            var blob = await ReferenceBlob(dataFile.ContainerName ?? "", dataFile.Name ?? "");
            if (blob != null)
            {
                await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);

                return await Result.SuccessAsync();
            }

            return await Result.FailAsync("File not found");
        }

        public async Task<Result<DownloadFileModel>> DownloadDataFileAsync(DataFile dataFile, CancellationToken cancellationToken)
        {
            var blob = await ReferenceBlob(dataFile.ContainerName ?? "", dataFile.Name ?? "");
            if (blob != null)
            {
                var stream = new MemoryStream();

                await blob.DownloadToAsync(stream, cancellationToken);

                stream.Seek(0, SeekOrigin.Begin);

                var result = new DownloadFileModel()
                {
                    Stream = stream,
                    ContentType = dataFile.ContentType,
                    FileName = dataFile.Name
                };

                return await Result<DownloadFileModel>.SuccessAsync(result);
            }

            return await Result<DownloadFileModel>.FailAsync("Fail to download");
        }

        public async Task<Tuple<Result, string>> UploadDataFileAsync<T>(UploadDataFileCommand command, CancellationToken cancellationToken) where T : UploadDataFileCommand
        {
            var containerName = "";

            #region Validation

            switch (typeof(T).Name)
            {
                case nameof(UploadUserProfileImageCommand):
                    containerName = _azureStorageConfig.UserProfileImageContainer;
                    break;
                default:
                    return new Tuple<Result, string>(
                    await Result.FailAsync("Command must be an extension of UploadDataFileCommand"),
                    containerName);
            }

            if (string.IsNullOrEmpty(containerName))
                return new Tuple<Result, string>(
                    await Result.FailAsync("Container name is null or empty"),
                    "");

            if (string.IsNullOrEmpty(command.FileName))
                return new Tuple<Result, string>(
                    await Result.FailAsync("File name is null or empty"),
                    "");

            if (command.File is null)
                return new Tuple<Result, string>(
                    await Result.FailAsync("No data file provided"),
                    "");

            #endregion

            #region Execution

            using var ms = new MemoryStream();
            command.File.CopyTo(ms);

            var blob = await ReferenceBlob(containerName, command.FileName, true);
            if (blob == null)
            {
                return new Tuple<Result, string>(await Result.FailAsync("Failed to reference blob"), containerName);
            }

            ms.Seek(0, SeekOrigin.Begin);
            await blob.UploadAsync(ms, cancellationToken);

            return new Tuple<Result, string>(await Result.SuccessAsync(), containerName);

            #endregion
        }

        #region Helpers

        /// <summary>
		/// Access the blob given the container and object name. Returns null if exception caught
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
