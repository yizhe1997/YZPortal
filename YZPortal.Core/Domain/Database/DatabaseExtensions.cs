using Microsoft.EntityFrameworkCore;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database.Users;

namespace YZPortal.Core.Domain.Database
{
    public static class DatabaseExtensions
    {
        #region User

        public static async Task<User?> GetUserByIdFirstOrDefaultAsync(this PortalContext portalContext, Guid id, CancellationToken cancellationToken = new CancellationToken()) => 
            await portalContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        public static async Task<User?> GetUserBySubIdFirstOrDefaultAsync(this PortalContext portalContext, Guid subId, CancellationToken cancellationToken = new CancellationToken()) =>
            await portalContext.Users.FirstOrDefaultAsync(u => u.SubjectIdentifier == subId, cancellationToken);

        public static IQueryable<User> GetUsersAsQueryable(this PortalContext portalContext) => portalContext.Users.AsQueryable();

        #endregion
    }
}
