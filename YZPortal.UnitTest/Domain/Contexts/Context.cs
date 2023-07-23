using Microsoft.EntityFrameworkCore;
using YZPortal.Core.Domain.Contexts;

namespace YZPortal.UnitTest.Domain.Contexts
{
    internal class Context
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
    }
}
