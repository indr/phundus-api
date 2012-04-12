using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    public class ScenarioTestBase : InMemoryDatabaseTestBase
    {
        public ScenarioTestBase() : base(typeof (Entity).Assembly)
        {
        }
    }
}