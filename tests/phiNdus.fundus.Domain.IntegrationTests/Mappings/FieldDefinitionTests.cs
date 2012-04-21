using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Mappings
{
    [TestFixture]
    public class FieldDefinitionTests : DomainComponentTestBase<FieldDefinition>
    {

        [Test]
        public void Can_save_and_load()
        {
            var sut = new FieldDefinition();
            sut.DataType = DataType.Integer;
            sut.Name = "Name";
            sut.IsDefault = true;
            sut.Position = 3;

            int id;
            using (var uow = UnitOfWork.Start())
            {
                UnitOfWork.CurrentSession.Save(sut);
                uow.TransactionalFlush();
                id = sut.Id;
            }

            using (UnitOfWork.Start())
            {
                var fromSession = UnitOfWork.CurrentSession.Get<FieldDefinition>(id);

                Assert.That(fromSession.DataType, Is.EqualTo(DataType.Integer));
                Assert.That(fromSession.Name, Is.EqualTo("Name"));
                Assert.That(fromSession.IsDefault, Is.True);
                Assert.That(fromSession.Position, Is.EqualTo(3));
            }
        }
    }
}