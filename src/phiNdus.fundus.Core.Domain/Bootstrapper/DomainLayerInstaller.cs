﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace phiNdus.fundus.Core.Domain.Bootstrapper
{
    public class DomainLayerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(new RepositoriesInstaller());
            container.Install(new NHibernateInstaller());
        }

        #endregion
    }
}