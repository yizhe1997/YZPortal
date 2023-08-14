using Microsoft.EntityFrameworkCore;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.EntityTypes.Users;
using YZPortal.Core.Domain.Database.EntityTypes.Users.Configs;

namespace YZPortal.Core.Domain.Database
{
    public static class DatabaseExtensions
    {
        #region Users

        public static async Task<User?> UserGetByIdFirstOrDefaultAsync(this PortalContext portalContext, Guid id, CancellationToken cancellationToken = new CancellationToken()) => 
            await portalContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        public static async Task<User?> UserGetBySubIdFirstOrDefaultAsync(this PortalContext portalContext, string? subId, CancellationToken cancellationToken = new CancellationToken()) =>
            await portalContext.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == subId, cancellationToken);

        public static IQueryable<User> UsersGetAsQueryable(this PortalContext portalContext) => portalContext.Users.AsQueryable();

        public static void UsersAdd(this PortalContext portalContext, List<User> users) => portalContext.Users.AddRange(users);

        public static void UsersDeleteAll(this PortalContext portalContext) => portalContext.Users.RemoveRange(portalContext.Users);

        public static void UserDelete(this PortalContext portalContext, User user) => portalContext.Users.Remove(user);

        public static int UsersGetCount(this PortalContext portalContext) => portalContext.Users.Count();

        #region Configs

        public static async Task<PortalConfig?> PortalConfigGetByUserSubIdFirstOrDefaultAsync(this PortalContext portalContext, string? userSubId, CancellationToken cancellationToken = new CancellationToken()) =>
            await portalContext.PortalConfigs.FirstOrDefaultAsync(u => u.UserSubjectIdentifier == userSubId, cancellationToken);

        #endregion

        #endregion
    }
}
