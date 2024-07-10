using Application.Models;
using Application.Models.File;
using Application.Requests;
using Domain.Entities.Media;

namespace Application.Interfaces.Services
{
    public interface IDataFileStorageService
    {
        Task<Tuple<Result, string>> UploadDataFileAsync<T>(UploadDataFileCommand command, CancellationToken cancellationToken = default) where T : UploadDataFileCommand;
        Task<Result> DeleteDataFileAsync(DataFile dataFile, CancellationToken cancellationToken = default);
        Task<Result<DownloadFileModel>> DownloadDataFileAsync(DataFile dataFile, CancellationToken cancellationToken = default);
    }
}
