using Application.Interfaces;
using Application.Interfaces.Services.Events;
using Application.Interfaces.Services.Identity;
using Application.Models;
using Application.Models.Identity;
using Application.Requests.Indexes;
using AutoMapper;
using Domain.Entities.Users;
using Domain.Events.Users;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Identity
{
    internal class UserService(
        ICurrentUserService currentUserService,
        UserManager<User> userManager,
        IEventPublisher eventPublisher,
        IMapper mapper) : IUserService
    {
        public async Task<SearchResult<UserModel>> GetSearchResultAsync(SearchRequest request, CancellationToken cancellationToken = default)
        {
            var query = userManager.Users.Include(x => x.UserProfileImage).Select(x => new User
            {
                Email = x.Email,
                DisplayName = x.DisplayName,
                FirstName = x.FirstName,
                LastName = x.LastName,
                SubjectIdentifier = x.SubjectIdentifier,
                IpAddress = x.IpAddress,
                AuthTime = x.AuthTime,
                AuthExpireTime = x.AuthExpireTime,
                AuthClassRef = x.AuthClassRef,
                MobilePhone = x.MobilePhone,
                IdentityProvider = x.IdentityProvider,
                LastidpAccessToken = x.LastidpAccessToken,
                UserProfileImageUrl = x.UserProfileImage.Url,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                UpdatedBy = x.UpdatedBy,
                UpdatedDate = x.UpdatedDate,
                Id = x.Id
            });

            var result = await SearchResult<User>.SuccessAsync<UserModel>(request, query, mapper, 
                x => x.DisplayName.Contains(request.SearchString) ||
                x.Email.Contains(request.SearchString),
                cancellationToken: cancellationToken);

            return result;
        }

        public async Task<IResult<UserModel>> GetBySubIdAsync(string? userSubId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == userSubId, cancellationToken);

            if (user == null)
                return await Result<UserModel>.FailAsync("User not found");

            var result = mapper.Map<UserModel>(user);
            return await Result<UserModel>.SuccessAsync(result);
        }

        public async Task<IResult<UserModel>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (user == null)
                return await Result<UserModel>.FailAsync("User not found");

            var result = mapper.Map<UserModel>(user);

            return await Result<UserModel>.SuccessAsync(result);
        }

        /// <summary>
        /// Generate an application user based on the current context.
        /// </summary>
        public async Task<IResult> CreateAsync<T>(T body, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == currentUserService.NameIdentifier, cancellationToken);

            if (user != null)
                return await Result<User>.FailAsync("User already exist");

            var newUser = mapper.Map<User>(body);

            var result = await userManager.CreateAsync(newUser);

            await eventPublisher.PublishAsync(new UserCreatedEvent(newUser));

            return await result.ToApplicationResultAsync();
        }

        /// <summary>
        /// Modifies the details of the application user.
        /// </summary>
        public async Task<IResult> UpdateAsync<T>(string? userSubId, T body, CancellationToken cancellationToken = default)
        {
            // Validate if user exist
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == userSubId, cancellationToken);
            if (user == null)
                return await Result<User>.FailAsync("User not found");

            // Map fields to existing user and save
            mapper.Map(body, user);

            var result = await userManager.UpdateAsync(user);

            return await result.ToApplicationResultAsync();
        }

        public async Task<IResult> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users.FirstAsync(u => u.Id == id, cancellationToken);

            if (user == null)
                return await Result<User>.FailAsync("User not found");

            var result = await userManager.DeleteAsync(user);

            return await result.ToApplicationResultAsync();
        }

        public async Task<IResult> DeleteBySubIdAsync(string? userSubId, CancellationToken cancellationToken = default)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == userSubId, cancellationToken);

            if (user == null)
                return await Result<User>.FailAsync("User not found");

            var result = await userManager.DeleteAsync(user);

            return await result.ToApplicationResultAsync();
        }
    }
}
