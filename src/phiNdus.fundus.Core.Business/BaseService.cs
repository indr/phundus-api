using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using phiNdus.fundus.Core.Domain;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business
{
    public class BaseService
    {
        static BaseService()
        {
            var container = new WindsorContainer();
            IoC.Initialize(container);

            // TODO,Inder: Mir ist schleierhaft, warum <mapping assembly="phiNdus.fundus.Core.Domain" /> im App.config nicht funktioniert.
            var factory = new NHibernateUnitOfWorkFactory(new Assembly[] { Assembly.GetAssembly(typeof(BaseEntity)) });

            container.Install(
                new RepositoriesInstaller());
            container.Register(Component.For<IUnitOfWorkFactory>().Instance(factory));
        }
    }
}
