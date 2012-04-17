using System;
using phiNdus.fundus.Core.Domain.Entities;

namespace phiNdus.fundus.TestHelpers.TestBases
{
    public class ComponentTestBase<TSut> : TestBase
    {
        public ComponentTestBase() : base()
        {

        }

        protected TSut Sut { get; set; }
    }
}
