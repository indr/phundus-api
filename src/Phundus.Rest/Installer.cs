namespace Phundus.Rest
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Api.Organizations;
    using AutoMapper;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);

            GlobalConfiguration.Configuration.DependencyResolver
                = new WindsorDependencyResolver(container);

            container.Register(Classes.FromThisAssembly()
                .BasedOn<IHttpController>()
                .If(t => t.Name.EndsWith("Controller", StringComparison.InvariantCulture))
                .LifestyleScoped());

            Mapper.AddProfile<OrganizationOrderDocsProfile>();
        }
    }
}