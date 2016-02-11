namespace Phundus.Web.Installers
{
    using Castle.Facilities.Logging;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;

    public class LoggerInstaller : IWindsorInstaller
    {
        #region IWindsorInstaller Members

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<LoggingFacility>(f =>
                                                   f.LogUsing(LoggerImplementation.Log4net)
                                                       .WithConfig("Web.config"));
        }

        #endregion
    }
}