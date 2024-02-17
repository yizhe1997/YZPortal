using Microsoft.AspNetCore.Http;
using Application.Interfaces.Services.Identity;
using Application.Extensions;
using Application.Interfaces.Services;

namespace Infrastructure.Services.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISerializerService _serializer;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, ISerializerService serializer)
        {
            _httpContextAccessor = httpContextAccessor;
            _serializer = serializer;
        }

        public string? NameIdentifier => _httpContextAccessor.HttpContext?.User.GetNameIdentifier();
        public string? DisplayName => _httpContextAccessor.HttpContext?.User.GetDisplayName();
        public string? FirstName => _httpContextAccessor.HttpContext?.User.GetFirstName();
        public string? LastName => _httpContextAccessor.HttpContext?.User.GetLastName();
        public string? AuthClassRef => _httpContextAccessor.HttpContext?.User.GetAuthClassRef();
        public string? Email => _httpContextAccessor.HttpContext?.User.GetEmail();
        public DateTime AuthTime => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(_httpContextAccessor.HttpContext?.User.GetAuthTime())).DateTime;
        public DateTime AuthExpireTime => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(_httpContextAccessor.HttpContext?.User.GetAuthExpireTime())).DateTime;
        public string? IdpAccessToken => _httpContextAccessor.HttpContext?.User.GetIdpAccessToken();
        public string? IdentityProvider => _httpContextAccessor.HttpContext?.User.GetIdentityProvider();
        public string? IpAddress => _httpContextAccessor.HttpContext?.Request.Host.ToUriComponent();
        //public IEnumerable<Claim> Claims => _httpContextAccessor.HttpContext?.User.Claims ?? new List<Claim>();
        public List<Domain.Entities.Users.Identity> Identities => _httpContextAccessor.HttpContext?.User.GetUserIdentities().Select(x => _serializer.Deserialize<Domain.Entities.Users.Identity>(x)).ToList() ?? new List<Domain.Entities.Users.Identity>();
    }
}
