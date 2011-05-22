using System;

namespace phiNdus.fundus.Core.Business.Security.Constraints
{
    public abstract class AbstractConstraint
    {
        public static AbstractConstraint operator &(AbstractConstraint c1, AbstractConstraint c2)
        {
            return new AndConstraint(c1, c2);
        }

        public static bool operator false(AbstractConstraint c1)
        {
            return false;
        }

        public static bool operator true(AbstractConstraint c1)
        {
            return false;
        }

        public static AbstractConstraint operator !(AbstractConstraint c1)
        {
            return new NotConstraint(c1);
        }

        public static AbstractConstraint operator |(AbstractConstraint c1, AbstractConstraint c2)
        {
            return new OrConstraint(c1, c2);
        }

        public abstract bool Eval(SecurityContext context);
    }
}