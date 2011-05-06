using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace phiNdus.fundus.Core.Domain
{
    public class RepositoriesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(AllTypes.FromAssemblyNamed("phiNdus.fundus.Core.Domain")
                                   .Where(type => type.Name.EndsWith("Repository"))
                                   .WithService.AllInterfaces()
                                   .Configure(c => c.LifeStyle.Transient));
        }
    }
}
