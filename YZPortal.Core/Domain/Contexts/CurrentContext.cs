using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YZPortal.Core.Domain.Database.Dealers;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.Core.Domain.Database.Users;

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

        public IEnumerable<Claim>? Claims => _httpContext?.User.Claims ?? default;
        public string NameClaim => Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

        #region Current User Context

        public Guid CurrentUserId => string.IsNullOrEmpty(NameClaim) ? Guid.Empty : Guid.Parse(NameClaim);
        public User? CurrentUser => _dbContext.Users.FirstOrDefault(u => u.Id == CurrentUserId);
        //public string ContextToken => CurrentMembership?.ContextToken;

        public IQueryable<Membership> CurrentUserMemberships => _dbContext.Memberships.Where(m => m.UserId == CurrentUserId);
        public IQueryable<Dealer> CurrentUserDealers => _dbContext.Dealers.Join(CurrentUserMemberships.Where(x => x.Disabled == false), d => d.Id, m => m.DealerId, (d, m) => d);

        #endregion

        #region Current Dealer Context

        public Guid CurrentDealerId => Claims != null ? Guid.TryParse(Claims.FirstOrDefault(x => x.Type == "dealerId")?.Value, out Guid result) ? result : Guid.Empty : Guid.Empty;
        public Dealer? CurrentDealer => CurrentDealerId != Guid.Empty ? _dbContext?.Dealers.FirstOrDefault(u => u.Id == CurrentDealerId) : null;
        public Membership? UserDealerMembership => CurrentUserId != Guid.Empty ? _dbContext?.Memberships.Include(x => x.User)
            .Include(x => x.Dealer)
            .Include(x => x.MembershipDealerRole)
            .ThenInclude(x => x.DealerRole)
            .Include(m => m.MembershipContentAccessLevels)
            .ThenInclude(m => m.ContentAccessLevel)
            .FirstOrDefault(x => x.DealerId == CurrentDealerId && x.UserId == CurrentUserId)
            : null;
        public IQueryable<MembershipInvite> CurrentDealerInvites => _dbContext.MembershipInvites.Where(i => i.DealerId == CurrentDealerId);
        public IQueryable<Membership> CurrentDealerMemberships => _dbContext.Memberships.Where(m => m.DealerId == CurrentDealerId);

        #endregion
    }
}
