using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Dealers;
using YZPortal.Core.Domain.Database.Memberships;

namespace YZPortal.Api.Controllers.Memberships
{
    public static class MembershipsHelper
    {
        public static readonly string emptyErrorMsg = false.ToString();

        /// <summary>
        ///     Updates roles and content access levels for a membership
        /// </summary>
        /// <param name="membership"></param>
        /// <param name="database"></param>
        /// <param name="userRoles"> defaults to representative</param>
        /// <param name="userAccessLevels"> defaults to all</param>
        public static void UpdateRolesAndContentAccessLevels(this Membership membership, PortalContext database, string userRoles = null, List<string> userAccessLevels = null)
        {
            if (userRoles == null)
                userRoles = DealerRoleTypes.Representative;
            if (userAccessLevels.Count() == 0)
            {
                userAccessLevels = new List<string>
                {
                    ContentAccessLevelTypes.All
                };
            }

            // Clear the roles and content access level for this membership each time it updates
            if (membership.MembershipDealerRole != null || membership.MembershipContentAccessLevels.Count() > 0)
            {
                membership.MembershipDealerRole = null;
                membership.MembershipContentAccessLevels.Clear();
                database.Update(membership);
                database.SaveChanges();
            }

            var role = database.DealerRoles.FirstOrDefault(x => x.Name == userRoles);

            if (role != null)
            {
                membership.MembershipDealerRole = new MembershipDealerRole() { DealerRoleId = role.Id };

                foreach (var userAccess in userAccessLevels)
                {
                    var accessLevel = database.ContentAccessLevels.FirstOrDefault(x => x.Name == userAccess);

                    if(membership.MembershipContentAccessLevels == null)
                        membership.MembershipContentAccessLevels = new List<MembershipContentAccessLevel>();

                    if (accessLevel != null)
                        membership.MembershipContentAccessLevels.Add(new MembershipContentAccessLevel { ContentAccessLevelId = accessLevel.Id });
                }
            }
        }

        public static void CreateUpdateMasterAdmin(List<Membership> membershipList, List<Dealer> dealers, Guid userId, bool checkMasterAdmin, PortalContext database)
        {
            if (checkMasterAdmin == true)
            {
                foreach (var dealer in dealers)
                {
                    var checkMember = membershipList.Where(x => x.DealerId == dealer.Id).FirstOrDefault();
                    if (checkMember == null)
                    {
                        var membership = new Membership { DealerId = dealer.Id, UserId = userId, Admin = true };
                        membership.UpdateRolesAndContentAccessLevels(database, DealerRoleTypes.Admin);
                        database.Memberships.Add(membership);
                    }
                    else
                    {
                        checkMember.Admin = true;
                        database.Memberships.Update(checkMember);
                    }
                }
            }
            else
            {
                foreach (var membership in membershipList)
                {
                    membership.Admin = false;
                    database.Memberships.Update(membership);
                }
            }

            database.SaveChanges();
        }

        #region CreateBulk

        public static string FormulateErrorMsgStringFormulator(this string errorMessage, int rowIndex, string errorMsgType)
        {
            return (errorMessage == emptyErrorMsg) ? $"WebApiWarningMessage:Line number {rowIndex} contains {errorMsgType}."
                : errorMessage.Remove(errorMessage.Length - 1, 1) + $", {errorMsgType}.";
        }

        public static List<CreateBulk.Sheet> FormulateErrorMsg(this CreateBulk.Sheet row,List<CreateBulk.Sheet> failedOutput, string errorMsgType)
        {
            row.HasError = true;

            if (failedOutput.FirstOrDefault(y => y.Index == row.Index) != null)
            {
                var fO = failedOutput.FirstOrDefault(y => y.Index == row.Index);
                fO.ErrorMsg = fO.ErrorMsg.FormulateErrorMsgStringFormulator(row.Index, errorMsgType);
            }
            else
            {
                row.ErrorMsg = row.ErrorMsg.FormulateErrorMsgStringFormulator(row.Index, errorMsgType);
                failedOutput.Add(row);
            }

            return failedOutput;
        }

        #endregion
    }
}
