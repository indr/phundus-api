using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace phiNdus.fundus.TestHelpers
{
    public class Pop3Exception : ApplicationException
    {
        public Pop3Exception(string str)
            : base(str)
        {
        }
    }
}
