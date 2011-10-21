using System.Linq;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Mappings
{
    [TestFixture]
    public class DomainObjectPersistenceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            Sut = new NHRepository<CompositeEntity>();
            PropertyDefinitionRepo = IoC.Resolve<IDomainPropertyDefinitionRepository>();


            using (var uow = UnitOfWork.Start())
            {
                NamePropertyDefinition = PropertyDefinitionRepo.Get(2);
                PricePropertyDefinition = PropertyDefinitionRepo.Get(4);

                UnitOfWork.CurrentSession.Delete("from CompositeEntity");
                uow.TransactionalFlush();
            }
        }

        #endregion

        protected FieldDefinition NamePropertyDefinition { get; set; }

        protected FieldDefinition PricePropertyDefinition { get; set; }

        protected IDomainPropertyDefinitionRepository PropertyDefinitionRepo { get; set; }

        protected IRepository<CompositeEntity> Sut { get; set; }


        [Test]
        public void Can_save_and_load_with_children()
        {
            var parentId = 0;
            var parent = new CompositeEntity();
            parent.AddProperty(NamePropertyDefinition, "Parent");
            
            var child1 = new CompositeEntity();
            child1.AddProperty(NamePropertyDefinition, "Child 1");
            parent.AddChild(child1);

            var child2 = new CompositeEntity();
            child2.AddProperty(NamePropertyDefinition, "Child 2");
            parent.AddChild(child2);

            var child2_1 = new CompositeEntity();
            child2_1.AddProperty(NamePropertyDefinition, "Child 2.1");
            child2.AddChild(child2_1);

            var child2_2 = new CompositeEntity();
            child2_2.AddProperty(NamePropertyDefinition, "Child 2.2");
            child2.AddChild(child2_2);

            using (var uow = UnitOfWork.Start())
            {
                Sut.Save(parent);
                parentId = parent.Id;
                uow.TransactionalFlush();
            }

            using (var uow = UnitOfWork.Start())
            {
                parent = Sut.Get(parentId);

                Assert.That(parent, Is.Not.Null);
                Assert.That(parent.Children, Has.Count.EqualTo(2));
                child1 = parent.Children.First();
                child2 = parent.Children.Last();
                Assert.That(child1.GetPropertyValue(NamePropertyDefinition), Is.EqualTo("Child 1"));
                Assert.That(child2.GetPropertyValue(NamePropertyDefinition), Is.EqualTo("Child 2"));

                Assert.That(child2.Children, Has.Count.EqualTo(2));
            }
        }

        [Test]
        public void Can_save_and_load_adding_properties()
        {
            var modelId = 0;
            var model = new CompositeEntity();
            model.AddProperty(NamePropertyDefinition);
            model.AddProperty(PricePropertyDefinition);

            using (var uow = UnitOfWork.Start())
            {
                Sut.Save(model);
                modelId = model.Id;
                uow.TransactionalFlush();
            }


            using (var uow = UnitOfWork.Start())
            {
                model = Sut.Get(modelId);
                Assert.That(model.HasProperty(NamePropertyDefinition));
                Assert.That(model.HasProperty(PricePropertyDefinition));
            }
        }

        [Test]
        public void Can_save_and_load_removing_properties()
        {
            var domainObjectId = 0;
            var domainObject = new CompositeEntity();
            domainObject.AddProperty(NamePropertyDefinition);
            domainObject.AddProperty(PricePropertyDefinition);

            // Insert
            using (var uow = UnitOfWork.Start())
            {
                Sut.Save(domainObject);
                domainObjectId = domainObject.Id;
                uow.TransactionalFlush();
            }

            // Update (Remove aus Collection)
            using (var uow = UnitOfWork.Start())
            {
                domainObject = Sut.Get(domainObjectId);
                domainObject.RemoveProperty(PricePropertyDefinition);
                Sut.Save(domainObject);
                uow.TransactionalFlush();
            }

            using (var uow = UnitOfWork.Start())
            {
                domainObject = Sut.Get(domainObjectId);
                Assert.That(domainObject.HasProperty(NamePropertyDefinition));
                Assert.That(domainObject.HasProperty(PricePropertyDefinition), Is.False);
            }
        }
    }
}