using System;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    public class ComponentTestBase<TSut> : InMemoryDatabaseTestBase
    {
        public ComponentTestBase() : base(typeof(Entity).Assembly)
        {
        }

        protected TSut Sut { get; set; }
    }
}
