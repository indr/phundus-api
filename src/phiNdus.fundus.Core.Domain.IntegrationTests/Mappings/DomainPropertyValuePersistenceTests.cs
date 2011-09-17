using System;
using NUnit.Framework;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Domain.IntegrationTests.Mappings
{
    [TestFixture]
    public class DomainPropertyValuePersistenceTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyRepo = IoC.Resolve<IDomainPropertyRepository>();
                BooleanProperty = propertyRepo.Get(1);
                Assert.That(BooleanProperty.Type, Is.EqualTo(DomainPropertyType.Boolean));
                TextProperty = propertyRepo.Get(2);
                Assert.That(TextProperty.Type, Is.EqualTo(DomainPropertyType.Text));
                IntegerProperty = propertyRepo.Get(3);
                Assert.That(IntegerProperty.Type, Is.EqualTo(DomainPropertyType.Integer));
                DecimalProperty = propertyRepo.Get(4);
                Assert.That(DecimalProperty.Type, Is.EqualTo(DomainPropertyType.Decimal));
                DateTimeProperty = propertyRepo.Get(5);
                Assert.That(DateTimeProperty.Type, Is.EqualTo(DomainPropertyType.DateTime));
            }
        }

        #endregion

        private DomainProperty BooleanProperty;
        private DomainProperty TextProperty;
        private DomainProperty IntegerProperty;
        private DomainProperty DecimalProperty;
        private DomainProperty DateTimeProperty;


        private static int Save(DomainProperty property, object value)
        {
            var result = 0;
            var propertyValue = new DomainPropertyValue(property);
            propertyValue.Value = value;

            using (var uow = UnitOfWork.Start())
            {
                UnitOfWork.CurrentSession.Save(propertyValue);
                result = propertyValue.Id;
                uow.TransactionalFlush();
            }
            return result;
        }

        private static object Load(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyValue = UnitOfWork.CurrentSession.Get<DomainPropertyValue>(id);
                Assert.That(propertyValue, Is.Not.Null);
                return propertyValue.Value;
            }
        }

        [Test]
        public void CanSaveAndLoadBooleanProperty()
        {
            var id = Save(BooleanProperty, true);
            var value = Load(id);
            Assert.That(value, Is.EqualTo(true));
        }

        [Test]
        public void CanSaveAndLoadDateTimeProperty()
        {
            var now = DateTime.Now;

            // TODO: datetime-Datentyp in MSSQL speichert keine Sekundenbruchteile
            now = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerSecond), now.Kind);

            var id = Save(DateTimeProperty, now);
            var value = Load(id);
            Assert.That(value, Is.EqualTo(now));
        }

        [Test]
        public void CanSaveAndLoadDecimalProperty()
        {
            var id = Save(DecimalProperty, 2.1d);
            var value = Load(id);
            Assert.That(value, Is.EqualTo(2.1d));
        }

        [Test]
        public void CanSaveAndLoadIntegerProperty()
        {
            var id = Save(IntegerProperty, 2);
            var value = Load(id);
            Assert.That(value, Is.EqualTo(2));
        }

        [Test]
        public void CanSaveAndLoadTextProperty()
        {
            var id = Save(TextProperty, "Dies ist ein Text!");
            var value = Load(id);
            Assert.That(value, Is.EqualTo("Dies ist ein Text!"));
        }
    }
}