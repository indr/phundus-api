using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace phiNdus.fundus.TestHelpers.Builders
{
    using piNuts.phundus.Infrastructure.Obsolete;

    public abstract class BuilderBase<T>
    {
        protected virtual void Persist(T obj)
        {
            if (UnitOfWork.IsStarted)
                UnitOfWork.CurrentSession.Save(obj);
        }
        
        public abstract T Build();
    }
}
