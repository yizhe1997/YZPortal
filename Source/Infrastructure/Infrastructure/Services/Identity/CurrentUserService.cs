using Microsoft.AspNetCore.Http;
using Application.Interfaces.Services.Identity;
using Application.Extensions;
using Application.Interfaces.Services;

namespace Infrastructure.Services.Identity
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor, ISerializerService serializer) : ICurrentUserService
    {
        public string? NameIdentifier => httpContextAccessor.HttpContext?.User.GetNameIdentifier();
        public string? DisplayName => httpContextAccessor.HttpContext?.User.GetDisplayName();
        public string? FirstName => httpContextAccessor.HttpContext?.User.GetFirstName();
        public string? LastName => httpContextAccessor.HttpContext?.User.GetLastName();
        public string? AuthClassRef => httpContextAccessor.HttpContext?.User.GetAuthClassRef();
        public string? Email => httpContextAccessor.HttpContext?.User.GetEmail();
        public DateTime AuthTime => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(httpContextAccessor.HttpContext?.User.GetAuthTime())).DateTime;
        public DateTime AuthExpireTime => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(httpContextAccessor.HttpContext?.User.GetAuthExpireTime())).DateTime;
        public string? IdpAccessToken => httpContextAccessor.HttpContext?.User.GetIdpAccessToken();
        public string? IdentityProvider => httpContextAccessor.HttpContext?.User.GetIdentityProvider();
        public string? IpAddress => httpContextAccessor.HttpContext?.Request.Host.ToUriComponent();
        //public IEnumerable<Claim> Claims => _httpContextAccessor.HttpContext?.User.Claims ?? new List<Claim>();
        public List<Domain.Entities.Users.Identity> Identities => httpContextAccessor.HttpContext?.User.GetUserIdentities().Select(x => serializer.Deserialize<Domain.Entities.Users.Identity>(x)).ToList() ?? new List<Domain.Entities.Users.Identity>();
    }
}
