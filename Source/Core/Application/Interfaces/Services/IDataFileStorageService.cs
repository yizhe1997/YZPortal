using Application.Models;
using Application.Models.File;
using Application.Requests;
using Domain.Entities.Misc;

namespace Application.Interfaces.Services
{
    public interface IDataFileStorageService
    {
        Task<Tuple<Result, string>> UploadDataFileAsync<T>(UploadDataFileCommand command, CancellationToken cancellationToken = new CancellationToken()) where T : UploadDataFileCommand;
        Task<Result> DeleteDataFileAsync(DataFile dataFile, CancellationToken cancellationToken = new CancellationToken());
        Task<Result<DownloadFileModel>> DownloadDataFileAsync(DataFile dataFile, CancellationToken cancellationToken = new CancellationToken());
    }
}
