using Microsoft.AspNetCore.Authorization;
using YZPortal.FullStackCore.Enums.Memberships;

namespace YZPortal.FullStackCore.Infrastructure.Security.Authorization
{
    public static class Policies
    {
        #region Roles

        #region Admin

        public const string Administrator = "Administrator";

        public static AuthorizationPolicy CreateAdministratorPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole(DealerRoleNames.Administrator.ToString())
                                                   .Build();
        }

        public static void AddAdministratorPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(Administrator, CreateAdministratorPolicy());
        }

        #endregion

        #region General

        public const string General = "General";

        public static AuthorizationPolicy CreateGeneralPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole(DealerRoleNames.General.ToString())
                                                   .Build();
        }

        public static void AddGeneralPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(General, CreateGeneralPolicy());
        }


        #endregion

        #endregion

        /// <summary>
        ///     Assigning Azure AD roles to the group requires Azure P1 plan. Too expensive for now.
        ///     So a user assigned to Administrator group will have admin rights to all assigned subscription for now...
        /// </summary>
        #region Subscription

        #region Badminton

        public const string Badminton = "Badminton";

        public static AuthorizationPolicy CreateBadmintonSubscriptionPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole(((int)ContentAccessLevelNames.All).ToString())
                                                   .Build();
        }

        public static void AddBadmintonSubscriptionPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(Badminton, CreateBadmintonSubscriptionPolicy());
        }

        #endregion

        #endregion
    }
}
