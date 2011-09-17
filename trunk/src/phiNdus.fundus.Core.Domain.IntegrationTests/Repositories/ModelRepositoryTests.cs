using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Repositories
{
    [TestFixture]
    public class ModelRepositoryTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = IoC.Resolve<IModelRepository>();
            PropertyRepo = IoC.Resolve<IDomainPropertyRepository>();


            using (var uow = UnitOfWork.Start())
            {
                NameProperty = PropertyRepo.Get(2);
                PriceProperty = PropertyRepo.Get(4);

                UnitOfWork.CurrentSession.Delete("from Model");
                uow.TransactionalFlush();
            }
        }

        #endregion

        protected DomainProperty NameProperty { get; set; }

        protected DomainProperty PriceProperty { get; set; }

        protected IDomainPropertyRepository PropertyRepo { get; set; }

        protected IModelRepository Sut { get; set; }


        [Test]
        public void Can_save_and_load_adding_properties()
        {
            var modelId = 0;
            var model = new Model();
            model.AddProperty(NameProperty);
            model.AddProperty(PriceProperty);

            using (var uow = UnitOfWork.Start())
            {
                Sut.Save(model);
                modelId = model.Id;
                uow.TransactionalFlush();
            }


            using (var uow = UnitOfWork.Start())
            {
                model = Sut.Get(modelId);
                Assert.That(model.HasProperty(NameProperty));
                Assert.That(model.HasProperty(PriceProperty));
            }
        }

        [Test]
        public void Can_save_and_load_removing_properties()
        {
            var modelId = 0;
            var model = new Model();
            model.AddProperty(NameProperty);
            model.AddProperty(PriceProperty);

            // Insert
            using (var uow = UnitOfWork.Start())
            {
                Sut.Save(model);
                modelId = model.Id;
                uow.TransactionalFlush();
            }

            // Update (Remove aus Collection)
            using (var uow = UnitOfWork.Start())
            {
                model = Sut.Get(modelId);
                model.RemoveProperty(PriceProperty);
                Sut.Save(model);
                uow.TransactionalFlush();
            }

            using (var uow = UnitOfWork.Start())
            {
                model = Sut.Get(modelId);
                Assert.That(model.HasProperty(NameProperty));
                Assert.That(model.HasProperty(PriceProperty), Is.False);
            }
        }
    }
}