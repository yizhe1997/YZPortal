using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Security.Claims;
using YZPortal.Core.Domain.Database.Users;
using YZPortal.FullStackCore.Extensions;

namespace YZPortal.Core.Domain.Contexts
{
    public class CurrentContext
    {
        private readonly PortalContext _dbContext;
        private readonly HttpContext? _httpContext;

        // Constructor
        public CurrentContext(PortalContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContext = httpContextAccessor.HttpContext;
        }

        #region Properties

        #region Claims

        public string? NameIdentifier => _httpContext?.User.GetNameIdentifier();
        public string? DisplayName => _httpContext?.User.GetDisplayName();
        public string? FirstName => _httpContext?.User.GetFirstName();
        public string? LastName => _httpContext?.User.GetLastName();
        public string? AuthClassRef => _httpContext?.User.GetAuthClassRef();
        public string? Email => _httpContext?.User.GetEmail();
        public DateTime AuthTime => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(_httpContext?.User.GetAuthTime())).DateTime;
        public DateTime AuthExpireTime => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(_httpContext?.User.GetAuthExpireTime())).DateTime;
        public string? IdpAccessToken => _httpContext?.User.GetIdpAccessToken();
        public string? IdentityProvider => _httpContext?.User.GetIdentityProvider();
        public string? IpAddress => _httpContext?.Request.Host.ToUriComponent();
        public IEnumerable<Claim>? Claims => _httpContext?.User.Claims ?? new List<Claim>();
        public List<Identity?> Identities => _httpContext?.User.GetUserIdentities().Select(x => JsonConvert.DeserializeObject<Identity>(x)).ToList() ?? new List<Identity?>();

        #endregion

        #region Current User Context

        public User? CurrentUser => _dbContext.Users.FirstOrDefault(u => u.SubjectIdentifier == NameIdentifier);

        #endregion

        #endregion
    }
}
