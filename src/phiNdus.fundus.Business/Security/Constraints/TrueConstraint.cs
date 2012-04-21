using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace phiNdus.fundus.Business.Security.Constraints
{
    class TrueConstraint : AbstractConstraint
    {
        public override string Message
        {
            get { throw new NotImplementedException(); }
        }

        public override bool Eval(SecurityContext context)
        {
            return true;
        }
    }
}
