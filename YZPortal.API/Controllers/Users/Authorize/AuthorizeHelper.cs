using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.Core.Error;
using YZPortal.FullStackCore.Enums.Memberships;
using YZPortal.FullStackCore.Infrastructure.Security.Authorization;

namespace YZPortal.API.Controllers.Users.Authorize
{
    public static class AuthorizeHelper
    {
        public static async Task<List<Claim>> CreateClaimsForMembership(this Membership? membership, PortalContext Database, CurrentContext CurrentContext)
        {
            #region Validate membership

            if (membership == null)
                throw new RestException(HttpStatusCode.Unauthorized, "Unrecognized dealer");

            if (membership.Disabled == true)
                throw new RestException(HttpStatusCode.Unauthorized, "Your account has been disabled. Please contact your system administrator");

            #endregion

            #region Create list of claim using this membership

            var claims = new List<Claim>
            {
                new Claim(Claims.UserdealerId, membership.DealerId.ToString())
            };

            if (membership.Admin || CurrentContext?.CurrentUser?.Admin == true)
            {
                // Assign admin role
                claims.Add(new Claim(ClaimTypes.Role, ((int)DealerRoleNames.Admin).ToString()));

                // Assign all access levels
                foreach (var accessLevel in await Database.ContentAccessLevels.ToListAsync())
                {
                    claims.Add(new Claim(Claims.MembershipAccessLevel, (accessLevel.Name).ToString()));
                }
            }
            else if (membership.MembershipDealerRole?.DealerRole != null && membership.MembershipContentAccessLevels.Any())
            {
                // Assign Role
                claims.Add(new Claim(ClaimTypes.Role, (membership.MembershipDealerRole.DealerRole.Name).ToString()));

                // Assign access Levels
                foreach (var memAccessLevel in membership.MembershipContentAccessLevels)
                {
                    if (memAccessLevel.ContentAccessLevel != null)
                        claims.Add(new Claim(Claims.MembershipAccessLevel, (memAccessLevel.ContentAccessLevel.Name).ToString()));
                }
            }
            else
            {
                // Assign empty role
                claims.Add(new Claim(ClaimTypes.Role, string.Empty));

                // Assign empty access Levels
                claims.Add(new Claim(Claims.MembershipAccessLevel, string.Empty));
            }

            #endregion

            return claims;
        }

    }
}
