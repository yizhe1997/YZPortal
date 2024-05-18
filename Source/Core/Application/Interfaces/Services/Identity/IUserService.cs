using Application.Models;
using Application.Models.Identity;
using Application.Requests.Indexes;

namespace Application.Interfaces.Services.Identity
{
    public interface IUserService
    {
        Task<SearchResult<UserModel>> GetSearchResultAsync(SearchRequest request, CancellationToken cancellationToken = new CancellationToken());
        
        Task<IResult<UserModel>> GetBySubIdAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken());

        Task<IResult<UserModel>> GetAsync(Guid id, CancellationToken cancellationToken = new CancellationToken());

        Task<IResult> CreateAsync<T>(T body, CancellationToken cancellationToken = new CancellationToken());

        Task<IResult> UpdateAsync<T>(string? userSubId, T body, CancellationToken cancellationToken = new CancellationToken());

        Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken = new CancellationToken());

        Task<IResult> DeleteBySubIdAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken());
    }
}
