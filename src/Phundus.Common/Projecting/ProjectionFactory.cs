namespace Phundus.Common.Projecting
{
    using System;
    using System.Reflection;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel;

    public interface IProjectionFactory
    {
        IProjection FindProjection(string fullName);
    }

    public class ProjectionFactory : IProjectionFactory
    {
        private readonly ITypedProjectionFactory _projectionFactory;

        public ProjectionFactory(ITypedProjectionFactory projectionFactory)
        {
            if (projectionFactory == null) throw new ArgumentNullException("projectionFactory");
            _projectionFactory = projectionFactory;
        }

        public IProjection FindProjection(string fullName)
        {
            try
            {
                return _projectionFactory.GetProjection(fullName);
            }
            catch (ComponentNotFoundException)
            {
                return null;
            }
        }
    }

    public interface ITypedProjectionFactory
    {
        IProjection GetProjection(string fullName);        
    }

    public class ProjectionSelector : DefaultTypedFactoryComponentSelector
    {
        public ProjectionSelector() : base(false)
        {
        }

        protected override string GetComponentName(MethodInfo method, object[] arguments)
        {
            if (arguments.Length == 1 && arguments[0] is string)
            {
                return (string) arguments[0];
            }
            return base.GetComponentName(method, arguments);
        }
    }
}