using Application.Models;
using Application.Models.Identity;
using Application.Requests.Indexes;

namespace Application.Interfaces.Services.Identity
{
    public interface IUserService
    {
        Task<SearchResult<UserModel>> GetSearchResultAsync(SearchRequest request, CancellationToken cancellationToken = default);
        
        Task<IResult<UserModel>> GetBySubIdAsync(string? userSubId, CancellationToken cancellationToken = default);

        Task<IResult<UserModel>> GetAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IResult> CreateAsync<T>(T body, CancellationToken cancellationToken = default);

        Task<IResult> UpdateAsync<T>(string? userSubId, T body, CancellationToken cancellationToken = default);

        Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IResult> DeleteBySubIdAsync(string? userSubId, CancellationToken cancellationToken = default);
    }
}
