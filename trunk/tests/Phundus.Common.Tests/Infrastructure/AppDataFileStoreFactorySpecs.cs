namespace Phundus.Common.Tests.Infrastructure
{
    using System;
    using Common.Infrastructure;
    using developwithpassion.specifications.extensions;
    using developwithpassion.specifications.rhinomocks;
    using Machine.Specifications;
    using Rhino.Mocks;

    public class app_data_file_storage_factory_concern : Observes<IFileStoreFactory, AppDataFileStoreFactory>
    {
        protected static IFileStore store;

        private Establish ctx = () =>
        {
            store = null;
            depends.on<IHostingEnvironment>().setup(x => x.MapPath(Arg<string>.Is.Anything))
                .Return((Func<string, string>) (s => s));
        };
    }

    [Subject(typeof (AppDataFileStoreFactory))]
    public class when_get_orders : app_data_file_storage_factory_concern
    {
        private Because of = () =>
            store = sut.GetOrders();

        private It should_have_base_directory = () =>
            store.BaseDirectory.ShouldEqual(@"~\App_Data\Storage\Orders");
    }

    [Subject(typeof (AppDataFileStoreFactory))]
    public class when_get_organizations : app_data_file_storage_factory_concern
    {
        private static Guid organizationId = Guid.NewGuid();

        private Because of = () =>
            store = sut.GetOrganizations(organizationId);

        private It should_have_base_directory = () =>
            store.BaseDirectory.ShouldEqual(@"~\App_Data\Storage\Organizations\" + organizationId.ToString("D"));
    }
}