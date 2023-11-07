using Application.Models;
using Application.Models.File;
using Application.Requests;

namespace Application.Interfaces.Services
{
    public interface IFileStorageService
    {
        Task<Result> UploadFileAsync(UploadFileCommand command, CancellationToken cancellationToken = new CancellationToken());
        Task<Result> DeleteFileAsync(Guid id, CancellationToken cancellationToken = new CancellationToken());
        Task<Result<DownloadFileModel>> DownloadFileAsync(Guid id, CancellationToken cancellationToken = new CancellationToken());
    }
}
