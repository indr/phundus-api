using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.Bootstrapper
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromThisAssembly()
                .BasedOn(typeof(IRepository<>))
                .WithService.AllInterfaces()
                .Configure(c => c.LifeStyle.Transient));
        }
    }
}
