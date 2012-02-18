using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.TestHelpers;

namespace phiNdus.fundus.Core.Domain.UnitTests
{
    public class TestBase : InMemoryDatabaseTestBase
    {
        public TestBase() : base(typeof (Entity).Assembly)
        {
        }
    }
}