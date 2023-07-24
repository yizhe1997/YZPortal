using Xunit.Abstractions;
using YZPortal.Core.Domain.Contexts;
using YZPortal.UnitTest.Domain.Contexts;

namespace YZPortal.UnitTest.UnitTests
{
    public class UnitTestBaseTemplate
    {
        protected readonly Fixture _fixture = new();
        protected readonly PortalContext _portalContext = PortalContextHelpers.CreatePortalContext();
        protected readonly int _userCount = 10000;
        protected readonly ITestOutputHelper _output;

        public UnitTestBaseTemplate(ITestOutputHelper output)
        {
            PortalContextHelpers.UsersAddToPortalContextViaAutoFixt(_fixture, _portalContext, _userCount);
            _output = output;
        }
    }
}
