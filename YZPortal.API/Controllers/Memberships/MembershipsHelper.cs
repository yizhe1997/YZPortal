using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.API.Controllers.Memberships
{
    public static class MembershipsHelper
    {
        //public async static Task CreateUpdateMasterAdmin(List<Membership> membershipList, List<Dealer> dealers, User User, Guid userId, string contextToken, bool checkMasterAdmin, DealerPortalContext dbContext, FunctionApiContext apiContext)
        //{
        //    if (checkMasterAdmin == true)
        //    {
        //        foreach (var dealer in dealers)
        //        {
        //            var checkMember = membershipList.Where(x => x.DealerId == dealer.Id).FirstOrDefault();
        //            if (checkMember == null)
        //            {
        //                try
        //                {
        //                    // Create a membership in FnO
        //                    var nameParts = User.Name.Split(" ");
        //                    var firstName = nameParts[0];
        //                    var lastName = nameParts.Length > 1 ? nameParts[1] : "";

        //                    var result = await apiContext.Post("CreateMember", body: new
        //                    {
        //                        FirstName = firstName,
        //                        LastName = lastName,
        //                        User.Email,
        //                        Dealer = dealer.CustomerAccount
        //                    });

        //                    contextToken = result.Headers.GetValues("User-Context-Token").First();
        //                }
        //                catch
        //                {
        //                }
        //                var membership = new Membership { DealerId = dealer.Id, UserId = userId, Admin = true, ContextToken = contextToken };
        //                await dbContext.Memberships.AddAsync(membership);
        //            }
        //            else
        //            {
        //                checkMember.Admin = checkMember.OriginalContextToken = true;
        //                dbContext.Memberships.Update(checkMember);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        foreach (var membership in membershipList)
        //        {
        //            if (membership.OriginalContextToken == false)
        //            {
        //                try
        //                {
        //                    Dictionary<string, object> queryParams = new Dictionary<string, object>();

        //                    // Admin Token
        //                    var adminToken = new Core.Domain.Database.Membership();

        //                    queryParams.Add("Token", membership.ContextToken);
        //                    // Delete membership in FunctionApi
        //                    var result = await apiContext.Post("deletedealeruser", adminToken.ContextToken, queryParams);
        //                }
        //                catch
        //                {
        //                }

        //                dbContext.Memberships.Remove(membership);
        //            }
        //            else
        //            {
        //                membership.Admin = membership.OriginalContextToken = false;
        //                dbContext.Memberships.Update(membership);
        //            }
        //        }
        //    }
        //    dbContext.SaveChanges();
        //}
        
        public static void UpdateRolesAndContentAccessLevels(this Membership membership, PortalContext database, int role, List<int> contentAccessLevels)
        {
            if (role <= 0)
                role = (int)DealerRoleNames.Representative;
            if (contentAccessLevels.Count() == 0)
                contentAccessLevels = new List<int> { (int)ContentAccessLevelNames.All };

            // Clear the roles and content access level for this membership each time it updates
            if (membership.MembershipDealerRole != null || membership.MembershipContentAccessLevels.Count() > 0)
            {
                membership.MembershipDealerRole = null;
                membership.MembershipContentAccessLevels.Clear();
            }

            var checkRole = database.DealerRoles.FirstOrDefault(x => x.Name == role);

            if (checkRole != null)
                membership.MembershipDealerRole = new MembershipDealerRole() { DealerRole = checkRole };

            foreach (var userAccess in contentAccessLevels)
            {
                var accessLevel = database.ContentAccessLevels.FirstOrDefault(x => x.Name == userAccess);

                if (accessLevel != null)
                    membership.MembershipContentAccessLevels.Add(new MembershipContentAccessLevel { ContentAccessLevel = accessLevel });
            }
        }
    }
}
