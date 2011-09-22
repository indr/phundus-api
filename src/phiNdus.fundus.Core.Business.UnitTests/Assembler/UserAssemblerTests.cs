using System;
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
    public class UserAssemblerTests : BaseTestFixture
    {
        #region Setup/Teardown

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            UserDto = new UserDto();
            UserDto.CreateDate = new DateTime(2011, 6, 5, 14, 48, 55);
            UserDto.Email = "john.wayne@example.com";
            UserDto.FirstName = "John";
            UserDto.Id = 1;
            UserDto.Version = 2;
            UserDto.IsApproved = true;
            UserDto.LastName = "Wayne";

            User = new User(1, 2);
            User.FirstName = "John";
            User.LastName = "Wayne";
            User.Role = new Role(1, "Benutzer");
            var membership = new DerivedMembership();
            membership.SetCreateDate(new DateTime(2011, 6, 5, 14, 48, 55));
            User.Membership = membership;
            User.Membership.Comment = "No one reads comments.";
            User.Membership.Email = "john.wayne@example.com";
            User.Membership.IsApproved = true;
            User.Membership.IsLockedOut = true;
            User.Membership.LastLockoutDate = null;
            User.Membership.LastLogOnDate = null;
            User.Membership.LastPasswordChangeDate = null;
        }

        #endregion

        private class DerivedMembership : Membership
        {
            public void SetCreateDate(DateTime value)
            {
                base.CreateDate = value;
            }
        }

        protected IUserRepository FakeUserRepository { get; set; }
        protected IRoleRepository FakeRoleRepository { get; set; }

        private User User { get; set; }
        private UserDto UserDto { get; set; }

        private void GenerateAndRegisterMissingStubs()
        {
            if (IoC.TryResolve<IUserRepository>() == null)
            {
                FakeUserRepository = GenerateAndRegisterStub<IUserRepository>();
                FakeUserRepository.Expect(x => x.Get(1)).Return(User);
            }

            if (IoC.TryResolve<IRoleRepository>() == null)
            {
                FakeRoleRepository = GenerateAndRegisterStub<IRoleRepository>();
                FakeRoleRepository.Expect(x => x.Get(1)).Return(Role.User);
                FakeRoleRepository.Expect(x => x.Get(2)).Return(Role.Administrator);
            }
        }

        

        [Test]
        public void CreateDomainObject_returns_new_domain_object()
        {
            GenerateAndRegisterMissingStubs();

            var domainObject = UserAssembler.CreateDomainObject(UserDto);

            Assert.That(domainObject, Is.Not.Null);
            Assert.That(domainObject.Id, Is.EqualTo(0));
            Assert.That(domainObject.FirstName, Is.EqualTo("John"));
            Assert.That(domainObject.LastName, Is.EqualTo("Wayne"));
        }

        [Test]
        public void CreateDomainObject_with_null_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => UserAssembler.CreateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void CreateDto_returns_correct_dto()
        {
            var dto = UserAssembler.CreateDto(User);

            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.Version, Is.EqualTo(2));
            Assert.That(dto.FirstName, Is.EqualTo("John"));
            Assert.That(dto.LastName, Is.EqualTo("Wayne"));
            Assert.That(dto.Email, Is.EqualTo("john.wayne@example.com"));
            Assert.That(dto.CreateDate, Is.EqualTo(new DateTime(2011, 6, 5, 14, 48, 55)));
            Assert.That(dto.IsApproved, Is.True);
            Assert.That(dto.RoleId, Is.EqualTo(1));
            Assert.That(dto.RoleName, Is.EqualTo("Benutzer"));
        }

        [Test]
        public void CreateDto_with_membership_null_throws()
        {
            User.Membership = null;
            Assert.Throws<ArgumentNullException>(() => UserAssembler.CreateDto(User));
        }

        [Test]
        public void CreateDto_with_null_subject_throws()
        {
            Assert.Throws<ArgumentNullException>(() => UserAssembler.CreateDto(null));
        }

        [Test]
        public void CreateDto_without_role_returns_dto()
        {
            User.Role = null;

            var dto = UserAssembler.CreateDto(User);

            Assert.That(dto.Id, Is.EqualTo(1));
            Assert.That(dto.RoleId, Is.EqualTo(0));
            Assert.That(dto.RoleName, Is.Null);
        }

        [Test]
        public void UpdateDomainObject_returns_correct_updated_domain_object()
        {
            GenerateAndRegisterMissingStubs();

            // Update Dto
            UserDto.RoleId = 2;

            var actual = UserAssembler.UpdateDomainObject(UserDto);

            // Updated
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Id, Is.GreaterThan(0));
            Assert.That(actual.FirstName, Is.EqualTo("John"));
            Assert.That(actual.LastName, Is.EqualTo("Wayne"));

            Assert.That(actual.Role, Is.Not.Null);
            Assert.That(actual.Role.Id, Is.EqualTo(2));
        }

        [Test]
        public void UpdateDomainObject_with_id_not_in_repository_throws()
        {
            FakeUserRepository = GenerateAndRegisterStub<IUserRepository>();
            GenerateAndRegisterMissingStubs();

            FakeUserRepository.Expect(x => x.Get(1)).Return(null);

            Assert.Throws<EntityNotFoundException>(() => UserAssembler.UpdateDomainObject(UserDto));
        }

        [Test]
        public void UpdateDomainObject_with_null_subject_throws()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => UserAssembler.UpdateDomainObject(null));
            Assert.That(ex.ParamName, Is.EqualTo("subject"));
        }

        [Test]
        public void UpdateDomainObject_with_version_not_equal_from_repository_throws()
        {
            GenerateAndRegisterMissingStubs();
            UserDto.Version = 1;

            Assert.Throws<DtoOutOfDateException>(() => UserAssembler.UpdateDomainObject(UserDto));
        }
    }
}