using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;
using YZPortal.Core.Error;

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
                new Claim("dealerId", membership.Dealer.Id.ToString())
            };

            if (membership.Admin || CurrentContext?.User?.Admin == true)
            {
                // set all roles
                foreach (var role in await Database.DealerRoles.Where(x => x.Name == (int)DealerRoleNames.Admin).ToListAsync())
                {
                    claims.Add(new Claim(DealerRoleNames.Admin.ToString(), "1"));
                }
                // set all accessLevels
                foreach (var accessLevel in await Database.ContentAccessLevels.ToListAsync())
                {
                    claims.Add(new Claim(((ContentAccessLevelNames)accessLevel.Name).ToString(), "1"));
                }
            }
            else if (membership.MembershipDealerRole?.DealerRole != null)
            {
                claims.Add(new Claim(((DealerRoleNames)membership.MembershipDealerRole.DealerRole.Name).ToString(), "1"));

                // Access Levels

                foreach (var mAccessLevel in membership.MembershipContentAccessLevels)
                {
                    claims.Add(new Claim(((ContentAccessLevelNames)mAccessLevel.ContentAccessLevel.Name).ToString(), "1"));
                }

                // Roles
                foreach (var role in await Database.DealerRoles.Where(x => x.Name != membership.MembershipDealerRole.DealerRole.Name).ToListAsync())
                {
                    claims.Add(new Claim(((DealerRoleNames)membership.MembershipDealerRole.DealerRole.Name).ToString(), "0"));
                }

                // Access Levels
                var currentAccessLevels = membership.MembershipContentAccessLevels.Select(y => y.ContentAccessLevel.Name).ToList();
                foreach (var accessLevel in await Database.ContentAccessLevels.Where(x => currentAccessLevels.Contains(x.Name) == false).ToListAsync())
                {
                    claims.Add(new Claim(((ContentAccessLevelNames)membership.MembershipDealerRole.DealerRole.Name).ToString(), "0"));
                }
            }
            else
            {
                // Roles
                foreach (var role in await Database.DealerRoles.ToListAsync())
                {
                    claims.Add(new Claim(((DealerRoleNames)role.Name).ToString(), "0"));
                }

                // Access Levels
                foreach (var accessLevel in await Database.ContentAccessLevels.ToListAsync())
                {
                    claims.Add(new Claim(((ContentAccessLevelNames)accessLevel.Name).ToString(), "0"));
                }
            }

            #endregion

            return claims;
        }

    }
}
