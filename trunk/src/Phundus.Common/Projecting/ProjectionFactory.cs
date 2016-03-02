namespace Phundus.Common.Projecting
{
    using System;
    using System.Reflection;
    using Castle.Facilities.TypedFactory;

    public interface IProjectionFactory
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