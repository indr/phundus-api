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
                var propertyRepo = IoC.Resolve<IFieldDefinitionRepository>();
                _booleanPropertyDefinition = propertyRepo.Get(1);
                Assert.That(_booleanPropertyDefinition.DataType, Is.EqualTo(DataType.Boolean));
                _textPropertyDefinition = propertyRepo.Get(2);
                Assert.That(_textPropertyDefinition.DataType, Is.EqualTo(DataType.Text));
                _integerPropertyDefinition = propertyRepo.Get(3);
                Assert.That(_integerPropertyDefinition.DataType, Is.EqualTo(DataType.Integer));
                _decimalPropertyDefinition = propertyRepo.Get(4);
                Assert.That(_decimalPropertyDefinition.DataType, Is.EqualTo(DataType.Decimal));
                _dateTimePropertyDefinition = propertyRepo.Get(5);
                Assert.That(_dateTimePropertyDefinition.DataType, Is.EqualTo(DataType.DateTime));
            }
        }

        #endregion

        private FieldDefinition _booleanPropertyDefinition;
        private FieldDefinition _textPropertyDefinition;
        private FieldDefinition _integerPropertyDefinition;
        private FieldDefinition _decimalPropertyDefinition;
        private FieldDefinition _dateTimePropertyDefinition;


        private static int Save(FieldValue propertyValue, object value)
        {
            var result = 0;
            propertyValue.Value = value;
            using (var uow = UnitOfWork.Start())
            {
                UnitOfWork.CurrentSession.Save(propertyValue);
                result = propertyValue.Id;
                uow.TransactionalFlush();
            }
            return result;            
        }

        private static int Save(FieldDefinition propertyDefinition, object value)
        {
            var propertyValue = new FieldValue(propertyDefinition);
            return Save(propertyValue, value);
        }

        private static object Load(int id)
        {
            using (var uow = UnitOfWork.Start())
            {
                var propertyValue = UnitOfWork.CurrentSession.Get<FieldValue>(id);
                Assert.That(propertyValue, Is.Not.Null);
                return propertyValue.Value;
            }
        }

        [Test]
        public void CanSaveAndLoadBooleanProperty()
        {
            var id = Save(_booleanPropertyDefinition, true);
            var value = Load(id);
            Assert.That(value, Is.EqualTo(true));
        }

        [Test]
        public void CanSaveAndLoadDateTimeProperty()
        {
            var now = DateTime.Now;

            // TODO: datetime-Datentyp in MSSQL speichert keine Sekundenbruchteile
            now = new DateTime(now.Ticks - (now.Ticks % TimeSpan.TicksPerSecond), now.Kind);

            var id = Save(_dateTimePropertyDefinition, now);
            var value = Load(id);
            Assert.That(value, Is.EqualTo(now));
        }

        [Test]
        public void CanSaveAndLoadDecimalProperty()
        {
            var id = Save(_decimalPropertyDefinition, 2.1d);
            var value = Load(id);
            Assert.That(value, Is.EqualTo(2.1d));
        }

        [Test]
        public void CanSaveAndLoadIntegerProperty()
        {
            var id = Save(_integerPropertyDefinition, 2);
            var value = Load(id);
            Assert.That(value, Is.EqualTo(2));
        }

        [Test]
        public void CanSaveAndLoadTextProperty()
        {
            var id = Save(_textPropertyDefinition, "Dies ist ein Text!");
            var value = Load(id);
            Assert.That(value, Is.EqualTo("Dies ist ein Text!"));
        }

        [Test]
        public void Can_save_and_load_PropertyAsDiscriminator()
        {
            var propertyValue = new FieldValue(_textPropertyDefinition);
            propertyValue.IsDiscriminator = true;
            var id = Save(propertyValue, "");

            using (var uow = UnitOfWork.Start())
            {
                propertyValue = UnitOfWork.CurrentSession.Get<FieldValue>(id);
                Assert.That(propertyValue, Is.Not.Null);
            }
            Assert.That(propertyValue.IsDiscriminator, Is.True);
        }
    }
}