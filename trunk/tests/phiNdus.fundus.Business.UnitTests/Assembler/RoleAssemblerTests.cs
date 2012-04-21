using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Business.Assembler;
using phiNdus.fundus.Domain.Entities;

namespace phiNdus.fundus.Business.UnitTests.Assembler {
    
    [TestFixture]
    public class RoleAssemblerTests {

        [SetUp]
        public void SetUp() {
            _domainObject = new Role(1, "Admin");
        }

        private Role _domainObject;

        [Test]
        public void CreateDto_with_null_subject_throws() {
            Assert.Throws<ArgumentNullException>(() => RoleAssembler.CreateDto(null));
        }

        [Test]
        public void CreateDto_returns_correct_dto() {
            var dto = RoleAssembler.CreateDto(_domainObject);

            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Name, Is.EqualTo("Admin"));
        }
    }
}
