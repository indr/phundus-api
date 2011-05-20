using System;

namespace phiNdus.fundus.Core.Business.IntegrationTests.TestHelpers
{
    public class Pop3Exception : ApplicationException
    {
        public Pop3Exception(string str)
            : base(str)
        {
        }
    }
}