using Application.Interfaces.Services.Identity;
using AutoMapper;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity
{
    public class IdentityService(
        CurrentUserService currentUserService,
        UserManager<User> userManager,
        IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IMapper mapper) : IIdentityService
    {
    }
}
