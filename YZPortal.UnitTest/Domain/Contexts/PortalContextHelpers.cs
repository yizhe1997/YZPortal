using Microsoft.EntityFrameworkCore;
using YZPortal.Core.Domain.Contexts;
using YZPortal.Core.Domain.Database;
using YZPortal.Core.Domain.Database.EntityTypes.Users;

namespace YZPortal.UnitTest.Domain.Contexts
{
    internal class PortalContextHelpers
    {
        internal static PortalContext CreatePortalContext()
        {
            // Create in-memory db
            var builder = new DbContextOptionsBuilder<PortalContext>();
            builder.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            var dbContextOptions = builder.Options;
            var _portalContext = new PortalContext(dbContextOptions);

            // Delete existing db before creating a new one
            _portalContext.Database.EnsureDeleted();
            _portalContext.Database.EnsureCreated();

            return _portalContext;
        }

        public static void UsersAddToPortalContextViaAutoFixt(Fixture fixture, PortalContext portalContext, int count)
        {
            var users = fixture.CreateMany<User>(count).ToList();
            portalContext.UsersAdd(users);
            portalContext.SaveChanges();
        }

        public static void UsersDeleteAllFromPortalContext(PortalContext portalContext)
        {
            portalContext.UsersDeleteAll();
            portalContext.SaveChanges();
        }
    }
}
