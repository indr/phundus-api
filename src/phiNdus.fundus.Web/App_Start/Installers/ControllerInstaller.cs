﻿namespace Phundus.Web.Installers
{
    using System;
    using System.Web.Mvc;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly().BasedOn<IController>()
                .If(t => t.Name.EndsWith("Controller", StringComparison.InvariantCulture))
                .LifestyleTransient());
        }
    }
}