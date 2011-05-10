using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using phiNdus.fundus.Core.Domain;
using phiNdus.fundus.Core.Domain.Installers;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Business
{
    public class BaseService
    {
        static BaseService()
        {
            /*
            IoC.Initialize(new WindsorContainer());

            // TODO,Inder: Mir ist schleierhaft, warum <mapping assembly="phiNdus.fundus.Core.Domain" /> im App.config nicht funktioniert.
            var factory = new NHibernateUnitOfWorkFactory(new Assembly[] { Assembly.GetAssembly(typeof(BaseEntity)) });

            IoC.Container.Install(
                new RepositoriesInstaller());
            container.Register(Component.For<IUnitOfWorkFactory>().Instance(factory));*/
        }
    }
}
