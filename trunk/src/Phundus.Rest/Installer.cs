namespace Phundus.Rest
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using AutoMapper;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using MappingProfiles;

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

            var profiles = Assembly.GetExecutingAssembly().GetTypes()
                .Where(p => typeof (Profile).IsAssignableFrom(p)).ToList();                
                
            foreach (var profile in profiles)
            {
                Mapper.AddProfile(Activator.CreateInstance(profile) as Profile);
            }
        }
    }
}