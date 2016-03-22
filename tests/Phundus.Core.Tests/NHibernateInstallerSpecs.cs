namespace Phundus.Tests
{
    using System;
    using System.Reflection;
    using Common.Infrastructure.Persistence;
    using Common.Infrastructure.Persistence.Installers;
    using developwithpassion.specifications.rhinomocks;
    using FluentNHibernate.Cfg;
    using Machine.Specifications;
    using NHibernate.Cfg;

    [Subject(typeof (NHibernateInstaller))]
    public class when_building_fluent : Observes<NHibernateInstaller>
    {
        private static FluentConfiguration result;

        private static Configuration cfg;

        private Establish ctx = () =>
            sut_factory.create_using(() =>
                new NHibernateInstaller(cfgOutputFileName));

        private Because of = () =>
        {
            result = sut.BuildFluent();
            cfg = typeof (FluentConfiguration).GetField("cfg", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(result) as Configuration;
            NHibernateInstaller.ChangeExposedConfiguration(cfg);
        };

        private It should_return_config_with_post_commit_publish_notification_listener = () =>
            cfg.EventListeners.PostCommitInsertEventListeners.ShouldContain(
                c => c.GetType() == typeof (PostCommitPublishNotificationListener));

        private static string cfgOutputFileName = Guid.NewGuid() + ".txt";
    }
}