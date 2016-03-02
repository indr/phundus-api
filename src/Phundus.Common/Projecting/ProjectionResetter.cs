namespace Phundus.Common.Projecting
{
    using System;
    using Castle.Windsor;

    public interface IProjectionResetter
    {
        void Reset(string typeName);
    }

    public class ProjectionResetter : IProjectionResetter
    {
        private readonly IWindsorContainer _container;

        public ProjectionResetter(IWindsorContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            _container = container;
        }

        public void Reset(string typeName)
        {
            throw new NotImplementedException();
        }
    }
}