﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Core.Business.SecuredServices;

namespace phiNdus.fundus.Core.Business
{
    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                                   .BasedOn<BaseSecuredService>()
                                   .WithService.AllInterfaces()
                                   .Configure(c => c.LifeStyle.Transient));
            container.Register(Component.For<IMailGateway>()
                                   .ImplementedBy(typeof (MailGateway))
                                   .LifeStyle.Transient);

            container.Install(new Domain.Installer());
        }

        #endregion
    }
}