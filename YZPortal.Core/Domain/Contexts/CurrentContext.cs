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
        private readonly PortalContext? _dbContext;
        private readonly HttpContext? _httpContext;

        // Constructor
        public CurrentContext(PortalContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContext = httpContextAccessor.HttpContext;
        }

        public IEnumerable<Claim>? Claims => _httpContext?.User.Claims ?? null;
        public string? NameClaim => Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? null;

        #region Current User Context

        public Guid UserId => string.IsNullOrEmpty(NameClaim) ? Guid.Empty : Guid.Parse(NameClaim);
        public User? User => _dbContext?.Users.FirstOrDefault(u => u.Id == UserId) ?? null;
        //public string ContextToken => CurrentMembership?.ContextToken;

        public IQueryable<Membership>? UserMemberships => _dbContext?.Memberships?.Where(m => m.User.Id == UserId) ?? null;
        public IQueryable<Dealer>? UserMembershipsDealers => UserMemberships != null ? _dbContext?.Dealers.Join(UserMemberships, d => d.Id, m => m.Dealer.Id, (d, m) => d) ?? null : null;

        #endregion

        #region Current Dealer Context

        public Guid DealerId => Claims != null ? Guid.TryParse(Claims.FirstOrDefault(x => x.Type == "dealerId")?.Value, out Guid result) ? result : Guid.Empty : Guid.Empty;
        public Dealer? Dealer => DealerId != Guid.Empty ? _dbContext?.Dealers.FirstOrDefault(u => u.Id == DealerId) : null;
        public Membership? UserDealerMembership => UserId != Guid.Empty ? _dbContext?.Memberships.Include(x => x.User)
            .Include(x => x.Dealer)
            .Include(x => x.MembershipDealerRole)
            .ThenInclude(x => x.DealerRole)
            .Include(m => m.MembershipContentAccessLevels)
            .ThenInclude(m => m.ContentAccessLevel)
            .FirstOrDefault(x => x.Dealer.Id == DealerId && x.User.Id == UserId)
            : null;
        public IQueryable<MembershipInvite>? DealerInvites => _dbContext?.MembershipInvites.Where(i => i.Dealer.Id == DealerId) ?? null;
        public IQueryable<Membership>? DealerMemberships => _dbContext?.Memberships.Where(m => m.Dealer.Id == DealerId) ?? null;
        public IQueryable<Dealer>? Dealers => DealerMemberships != null ? _dbContext?.Dealers.Join(DealerMemberships.Where(x => x.Disabled == false), d => d.Id, m => m.Dealer.Id, (d, m) => d) : null;

        #endregion
    }
}
