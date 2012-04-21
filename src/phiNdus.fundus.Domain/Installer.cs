﻿using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using phiNdus.fundus.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Domain
{
    public class Installer : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyContaining(typeof (Entity))
                                   .BasedOn(typeof (IRepository<>))
                                   .WithService.AllInterfaces()
                                   .Configure(c => c.LifeStyle.Transient));


            container.Register(Component.For<IUnitOfWorkFactory>()
                                   .Instance(
                                       new NHibernateUnitOfWorkFactory(new[] {Assembly.GetAssembly(typeof (Entity))})));
        }

        #endregion
    }
}