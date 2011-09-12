using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Assembler;
using phiNdus.fundus.Core.Business.Dto;
using phiNdus.fundus.Core.Domain.Entities;
using phiNdus.fundus.Core.Domain.Repositories;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.Core.Business.UnitTests.Assembler
{
    [TestFixture]
    public class UserAssemblerTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _domainObject = new User(1, 2);
            _domainObject.FirstName = "John";
            _domainObject.LastName = "Wayne";
            var membership = new DerivedMembership();
            membership.SetCreateDate(new DateTime(2011, 6, 5, 14, 48, 55));
            _domainObject.Membership = membership;
            _domainObject.Membership.Comment = "No one reads comments.";
            _domainObject.Membership.Email = "john.wayne@example.com";
            _domainObject.Membership.IsApproved = true;
            _domainObject.Membership.IsLockedOut = true;
            _domainObject.Membership.LastLockoutDate = null;
            _domainObject.Membership.LastLogOnDate = null;
            _domainObject.Membership.LastPasswordChangeDate = null;

            _dto = new UserDto();
            _dto.CreateDate = new DateTime(2011, 6, 5, 14, 48, 55);
            _dto.Email = "john.wayne@example.com";
            _dto.FirstName = "John";
            _dto.Id = 1;
            _dto.Version = 2;
            _dto.IsApproved = true;
            _dto.LastName = "Wayne";
            _dto.Version = 0;

            MockFactory = new MockRepository();
            MockUserRepository = MockFactory.StrictMock<IUserRepository>();

            IoC.Initialize(new WindsorContainer());
            IoC.Container.Register(Component.For<IUserRepository>().Instance(MockUserRepository));
        }

        [TearDown]
        public void TearDown()
        {
            IoC.Container.Dispose();
        }

        #endregion

        private class DerivedMembership : Membership
        {
            public void SetCreateDate(DateTime value)
            {
                base.CreateDate = value;
            }
        }

        protected MockRepository MockFactory { get; set; }
        protected IUserRepository MockUserRepository { get; set; }

        private User _domainObject;
        private UserDto _dto;

        [Test]
        public void CreateDomainObjectWithNullSubjectThrowsCreateNullSubjectThrows()
        {
            Assert.Throws<ArgumentNullException>(() => UserAssembler.CreateDomainObject(null));
        }

        [Test]
        public void CreateDomainObject_returns_new_domain_object()
        {
            var domainObject = UserAssembler.CreateDomainObject(_dto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(0));
            Assert.That(domainObject.FirstName, Is.EqualTo("John"));
            Assert.That(domainObject.LastName, Is.EqualTo("Wayne"));
        }

        [Test]
        public void CreateDto_returns_correct_dto()
        {
            var dto = UserAssembler.CreateDto(_domainObject);

            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.FirstName, Is.EqualTo("John"));
            Assert.That(dto.LastName, Is.EqualTo("Wayne"));
            Assert.That(dto.Email, Is.EqualTo("john.wayne@example.com"));
            Assert.That(dto.CreateDate, Is.EqualTo(new DateTime(2011, 6, 5, 14, 48, 55)));
            Assert.That(dto.IsApproved, Is.True);
        }

        [Test]
        public void CreateDto_with_membership_null_throws()
        {
            _domainObject.Membership = null;
            Assert.Throws<ArgumentNullException>(() => UserAssembler.CreateDto(_domainObject));
        }

        [Test]
        public void CreateDto_with_null_subject_throws()
        {
            Assert.Throws<ArgumentNullException>(() => UserAssembler.CreateDto(null));
        }

        [Test]
        public void UpdateDomainObject_returns_correct_updated_domain_object()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(1)).Return(new User(1));
            }

            using (MockFactory.Playback())
            {
                var domainObject = UserAssembler.UpdateDomainObject(_dto);

                // Updated
                Assert.That(domainObject, Is.Not.Null);
                Assert.That(domainObject.Id, Is.GreaterThan(0));
                Assert.That(domainObject.FirstName, Is.EqualTo("John"));
                Assert.That(domainObject.LastName, Is.EqualTo("Wayne"));

                // Unchanged
                Assert.That(domainObject.Membership.Comment, Is.Null);
                Assert.That(domainObject.Membership.CreateDate, Is.Not.EqualTo(new DateTime(2011, 6, 5, 14, 48, 55)));
            }
        }

        [Test]
        public void UpdateDomainObject_with_id_not_in_repository_throws()
        {
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(1)).Return(null);
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<EntityNotFoundException>(() => UserAssembler.UpdateDomainObject(_dto));
            }
        }

        [Test]
        public void UpdateDomainObject_with_null_subject_throws()
        {
            Assert.Throws<ArgumentNullException>(() => UserAssembler.UpdateDomainObject(null));
        }

        [Test]
        public void UpdateDomainObject_with_version_not_equal_from_repository_throws()
        {
            _dto.Version = 1;
            using (MockFactory.Record())
            {
                Expect.Call(MockUserRepository.Get(1)).Return(new User());
            }
            using (MockFactory.Playback())
            {
                Assert.Throws<DtoOutOfDateException>(() => UserAssembler.UpdateDomainObject(_dto));
            }
        }
    }
}