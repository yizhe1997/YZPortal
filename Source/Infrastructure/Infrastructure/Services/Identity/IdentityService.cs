using Application.Interfaces.Services.Identity;
using AutoMapper;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly CurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        private readonly IUserClaimsPrincipalFactory<User> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMapper _mapper;

        public IdentityService(
            CurrentUserService currentUserService,
            UserManager<User> userManager,
            IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            IMapper mapper)
        {
            _currentUserService = currentUserService;
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _mapper = mapper;
        }
    }
}
