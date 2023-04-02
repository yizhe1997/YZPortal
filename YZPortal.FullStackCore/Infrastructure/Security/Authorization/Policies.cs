using Microsoft.AspNetCore.Authorization;
using YZPortal.FullStackCore.Enums.Memberships;

namespace YZPortal.FullStackCore.Infrastructure.Security.Authorization
{
    public static class Policies
    {
        #region DealerId

        public const string Dealer = "Dealer";

        public static AuthorizationPolicy DealerIdPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireClaim(Claims.UserdealerId)
                                                   .Build();
        }

        public static void AddDealerIdPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(Dealer, DealerIdPolicy());
        }

        #endregion

        #region Dealer Roles

        public const string IsAdmin = "IsAdmin";

        public static AuthorizationPolicy IsRoleAdminPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole(((int)DealerRoleNames.Admin).ToString())
                                                   .Build();
        }

        public static void AddIsRoleAdminPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(IsAdmin, IsRoleAdminPolicy());
        }

        #endregion

        #region Content Access Levels

        public const string Test = "Test";

        public static AuthorizationPolicy IsAccessLevelUserPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole(((int)ContentAccessLevelNames.All).ToString())
                                                   .Build();
        }

        public static void AddIsAccessLvlUserPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(Test, IsAccessLevelUserPolicy());

        }
        #endregion
    }
}
