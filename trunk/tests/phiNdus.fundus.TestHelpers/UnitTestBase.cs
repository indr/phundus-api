using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace phiNdus.fundus.TestHelpers
{
    public class UnitTestBase
    {

        [SetUp]
        public virtual void SetUp()
        {
            
        }

        protected virtual void GenerateMissingStubs()
        {
        }
    }
}
