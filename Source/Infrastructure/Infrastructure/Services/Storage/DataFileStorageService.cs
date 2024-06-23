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
using Azure.Storage.Blobs.Models;

namespace Infrastructure.Services.Storage
{
    public class DataFileStorageService(BlobServiceClient blobClient, IOptions<AzureStorageConfig> azureStorageConfig) : IDataFileStorageService
    {
        public async Task<Result> DeleteDataFileAsync(DataFile dataFile, CancellationToken cancellationToken = default)
        {
            var blobClient = new BlobClient(new Uri(dataFile.Url ?? ""));

            if (await blobClient.ExistsAsync(cancellationToken))
            {
                await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken, snapshotsOption: DeleteSnapshotsOption.IncludeSnapshots);

                return await Result.SuccessAsync();
            }

            return await Result.FailAsync("Data file not found");
        }

        public async Task<Result<DownloadFileModel>> DownloadDataFileAsync(DataFile dataFile, CancellationToken cancellationToken = default)
        {
            var blobClient = new BlobClient(new Uri(dataFile.Url ?? ""));

            if (await blobClient.ExistsAsync(cancellationToken))
            {
                var stream = new MemoryStream();

                await blobClient.DownloadToAsync(stream, cancellationToken);

                stream.Seek(0, SeekOrigin.Begin);

                var result = new DownloadFileModel()
                {
                    Stream = stream,
                    ContentType = dataFile.ContentType,
                    FileName = dataFile.Name
                };

                return await Result<DownloadFileModel>.SuccessAsync(result);
            }

            return await Result<DownloadFileModel>.FailAsync("Data file not found");
        }

        /// <returns>
        /// Item1: generic result
        /// Item2: container name used for uploading data file
        /// Item3: data file url
        /// </returns>
        public async Task<Tuple<Result, string>> UploadDataFileAsync<T>(UploadDataFileCommand command, CancellationToken cancellationToken = default) where T : UploadDataFileCommand
        {
            var containerName = "";
            var fileName = command.RefId.ToString() + "_" + command.File?.FileName;

            #region Validation

            switch (typeof(T).Name)
            {
                case nameof(UploadUserProfileImageCommand):
                    containerName = azureStorageConfig.Value.UserProfileImageContainer;
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

            if (string.IsNullOrEmpty(command.File?.FileName))
                return new Tuple<Result, string>(
                    await Result.FailAsync("File name is null or empty"),
                    "");

            if (command.File is null)
                return new Tuple<Result, string>(
                    await Result.FailAsync("No data file provided"),
                    "");

            #endregion

            #region Execution

            var containerClient = blobClient.GetBlobContainerClient(containerName);

            if (await containerClient.ExistsAsync(cancellationToken))
            {
                var blobClient = containerClient.GetBlobClient(fileName);

                if (await blobClient.ExistsAsync(cancellationToken))
                {
                    await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: cancellationToken);
                }

                // For now the ACL via SASUrl is sufficient
                var uri = blobClient.GenerateSasUri(BlobSasPermissions.Create | BlobSasPermissions.Read | BlobSasPermissions.Delete, DateTime.MaxValue);
                blobClient = new BlobClient(uri);

                using var ms = new MemoryStream();
                command.File.CopyTo(ms);

                ms.Seek(0, SeekOrigin.Begin);
                await blobClient.UploadAsync(ms, new BlobHttpHeaders { ContentType = command.File.ContentType }, cancellationToken: cancellationToken);

                return new Tuple<Result, string>(await Result.SuccessAsync(blobClient.Uri.ToString()), containerName);
            }

            return new Tuple<Result, string>(await Result.FailAsync("Failed to reference blob"), containerName);

            #endregion
        }
    }
}
