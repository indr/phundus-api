﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Core.Domain.Bootstrapper;

namespace phiNdus.fundus.Core.Business.Bootstrapper
{
    public class BusinessLayerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Install(new ServicesInstaller());
            container.Install(new GatewaysInstaller());

            container.Install(new DomainLayerInstaller());
        }

        #endregion
    }
}