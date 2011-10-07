using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Core.Business.Paging;

namespace phiNdus.fundus.Core.Business.UnitTests.Paging {

    [TestFixture]
    public class PagedResultTests {

        [Test]
        public void Throws_when_no_PageIndex_is_not_provided() {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                PagedResult<DomainEntity>.For(null, new List<DomainEntity>(), 0));

            Assert.That(exception, Is.Not.Null);
        }

        [Test]
        public void Throws_when_items_is_not_provided() {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                PagedResult<DomainEntity>.For<DomainEntity>(new PageIndex(), null, 0));

            Assert.That(exception, Is.Not.Null);
        }

        [Test]
        public void Can_be_created_from_PageIndex() {
            var pageIndex = new PageIndex { Index = 3, PageSize = 20 };
            var pagedResult = PagedResult<DomainEntity>.For(pageIndex,
                new List<DomainEntity> { new DomainEntity(1) }, 943);

            Assert.That(pagedResult, Is.Not.Null);
            Assert.That(pagedResult.PageSize, Is.EqualTo(20));
            Assert.That(pagedResult.PageIndex, Is.EqualTo(3));
            Assert.That(pagedResult.Items, Is.Not.Null);
            Assert.That(pagedResult.Items.Count, Is.EqualTo(1));
            Assert.That(pagedResult.Total, Is.EqualTo(943));
        }

        private class DomainEntity {
            public DomainEntity(int id) {
                this.Id = id;
            }

            public int Id { get; set; }
        }

    }
}
