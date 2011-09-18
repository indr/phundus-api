using System.Linq;
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
            PropertyDefinitionRepo = IoC.Resolve<IDomainPropertyDefinitionRepository>();


            using (var uow = UnitOfWork.Start())
            {
                NamePropertyDefinition = PropertyDefinitionRepo.Get(2);
                PricePropertyDefinition = PropertyDefinitionRepo.Get(4);

                UnitOfWork.CurrentSession.Delete("from Model");
                uow.TransactionalFlush();
            }
        }

        #endregion

        protected DomainPropertyDefinition NamePropertyDefinition { get; set; }

        protected DomainPropertyDefinition PricePropertyDefinition { get; set; }

        protected IDomainPropertyDefinitionRepository PropertyDefinitionRepo { get; set; }

        protected IModelRepository Sut { get; set; }


        [Test]
        public void Can_save_and_load_with_children()
        {
            var parentId = 0;
            var parent = new Model();
            parent.AddProperty(NamePropertyDefinition, "Parent");
            
            var child1 = new Model();
            child1.AddProperty(NamePropertyDefinition, "Child 1");
            parent.AddChild(child1);

            var child2 = new Model();
            child2.AddProperty(NamePropertyDefinition, "Child 2");
            parent.AddChild(child2);

            var child2_1 = new Model();
            child2_1.AddProperty(NamePropertyDefinition, "Child 2.1");
            child2.AddChild(child2_1);

            var child2_2 = new Model();
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
            var model = new Model();
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
            var modelId = 0;
            var model = new Model();
            model.AddProperty(NamePropertyDefinition);
            model.AddProperty(PricePropertyDefinition);

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
                model.RemoveProperty(PricePropertyDefinition);
                Sut.Save(model);
                uow.TransactionalFlush();
            }

            using (var uow = UnitOfWork.Start())
            {
                model = Sut.Get(modelId);
                Assert.That(model.HasProperty(NamePropertyDefinition));
                Assert.That(model.HasProperty(PricePropertyDefinition), Is.False);
            }
        }
    }
}