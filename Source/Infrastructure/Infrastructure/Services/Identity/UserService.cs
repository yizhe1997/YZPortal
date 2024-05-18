using Application.Interfaces;
using Application.Interfaces.Services.Identity;
using Application.Models;
using Application.Models.Identity;
using Application.Requests.Indexes;
using AutoMapper;
using Domain.Entities.Users;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Identity
{
    internal class UserService : IUserService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(
            ICurrentUserService currentUserService,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<SearchResult<UserModel>> GetSearchResultAsync(SearchRequest request, CancellationToken cancellationToken = new CancellationToken())
        {
            var query = _userManager.Users;
            var result = await SearchResult<User>.SuccessAsync<UserModel>(request, query, _mapper, 
                x => x.DisplayName.Contains(request.SearchString) ||
                x.UserName.Contains(request.SearchString),
                cancellationToken: cancellationToken);
            return result;
        }

        public async Task<IResult<UserModel>> GetBySubIdAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == userSubId, cancellationToken);

            if (user == null)
                return await Result<UserModel>.FailAsync("User not found");

            var result = _mapper.Map<UserModel>(user);
            return await Result<UserModel>.SuccessAsync(result);
        }

        public async Task<IResult<UserModel>> GetAsync(Guid id, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user == null)
                return await Result<UserModel>.FailAsync("User not found");

            var result = _mapper.Map<UserModel>(user);

            return await Result<UserModel>.SuccessAsync(result);
        }

        /// <summary>
        /// Generate an application user based on the current context.
        /// </summary>
        public async Task<IResult> CreateAsync<T>(T body, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == _currentUserService.NameIdentifier, cancellationToken);

            if (user != null)
                return await Result<User>.FailAsync("User already exist");

            var newUser = _mapper.Map<User>(body);

            var result = await _userManager.CreateAsync(newUser);

            return await result.ToApplicationResultAsync();
        }

        /// <summary>
        /// Modifies the details of the application user.
        /// </summary>
        public async Task<IResult> UpdateAsync<T>(string? userSubId, T body, CancellationToken cancellationToken = new CancellationToken())
        {
            // Validate if user exist
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == userSubId, cancellationToken);
            if (user == null)
                return await Result<User>.FailAsync("User not found");

            // Map fields to existing user and save
            _mapper.Map(body, user);

            var result = await _userManager.UpdateAsync(user);

            return await result.ToApplicationResultAsync();
        }

        public async Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == id, cancellationToken);

            if (user == null)
                return await Result<User>.FailAsync("User not found");

            var result = await _userManager.DeleteAsync(user);

            return await result.ToApplicationResultAsync();
        }

        public async Task<IResult> DeleteBySubIdAsync(string? userSubId, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == userSubId, cancellationToken);

            if (user == null)
                return await Result<User>.FailAsync("User not found");

            var result = await _userManager.DeleteAsync(user);

            return await result.ToApplicationResultAsync();
        }
    }
}
