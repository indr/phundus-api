using Castle.Facilities.NHibernate;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NHibernateFacility = Castle.Facilities.NHibernateIntegration.NHibernateFacility;

namespace piNuts.phundus.Infrastructure.App_Start
{
    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<INHibernateInstaller>()
                .ImplementedBy<NHibernateInstaller>());
            container.AddFacility<NHibernateFacility>();
        }

        #endregion
    }
}