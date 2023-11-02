using Domain.Enums.Memberships;
using Microsoft.AspNetCore.Authorization;

namespace Application.Extensions
{
    // TODO: clean this 
    public static class AuthorizationOptionsExtensions
    {
        #region Roles

        #region Admin

        public static AuthorizationPolicy CreateAdministratorPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole(DealerRoleNames.Administrator.ToString())
                                                   .Build();
        }

        public static void AddAdministratorPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(DealerRoleNames.Administrator.ToString(), CreateAdministratorPolicy());
        }

        #endregion

        #region General

        public static AuthorizationPolicy CreateGeneralPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole(DealerRoleNames.General.ToString())
                                                   .Build();
        }

        public static void AddGeneralPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(DealerRoleNames.General.ToString(), CreateGeneralPolicy());
        }


        #endregion

        #endregion

        /// <summary>
        ///     Assigning Azure AD roles to the group requires Azure P1 plan. Too expensive for now.
        ///     So a user assigned to Administrator group will have admin rights to all assigned subscription for now...
        /// </summary>
        #region Subscription

        #region Badminton

        public static AuthorizationPolicy CreateBadmintonSubscriptionPolicy()
        {
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                                                   .RequireRole(ContentAccessLevelNames.Badminton.ToString())
                                                   .Build();
        }

        public static void AddBadmintonSubscriptionPolicy(this AuthorizationOptions opts)
        {
            opts.AddPolicy(ContentAccessLevelNames.All.ToString(), CreateBadmintonSubscriptionPolicy());
        }

        #endregion

        #endregion
    }
}
