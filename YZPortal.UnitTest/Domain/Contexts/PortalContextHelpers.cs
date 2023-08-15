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
            // https://github.com/AutoFixture/AutoFixture/discussions/1229
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // https://stackoverflow.com/questions/18346803/how-can-i-instruct-autofixture-to-not-bother-filling-out-some-properties
            var users = fixture.Build<User>().Without(p => p.Identities).Without(p => p.PortalConfig).CreateMany(count).ToList();
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
