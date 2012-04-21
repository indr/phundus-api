using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using phiNdus.fundus.Web.State;
using phiNdus.fundus.TestHelpers;
using System.Web;
using Castle.MicroKernel.Registration;
using Rhino.Mocks;

namespace phiNdus.fundus.Web.UnitTests.State {

    [TestFixture]
    public class SessionStateManagerTests : MockTestBase<IStateManager> {

        protected override IStateManager CreateSut() {
            return new SessionStateManager();
        }

        private HttpContextBase HttpContextStub { get; set; }
        private HttpSessionStateBase SessionMock { get; set; }

        protected override void RegisterDependencies(Castle.Windsor.IWindsorContainer container) {
            base.RegisterDependencies(container);

            this.HttpContextStub = this.MockFactory.Stub<HttpContextBase>();
            this.SessionMock = this.MockFactory.StrictMock<HttpSessionStateBase>();

            this.HttpContextStub.Stub(s => s.Session)
                .Return(this.SessionMock);

            this.MockFactory.Replay(this.HttpContextStub);

            container.Register(
                Component.For<HttpContextBase>().Instance(this.HttpContextStub));
        }

        [Test]
        public void Load_non_existing_state_should_create_new() {
            this.SessionMock.Expect(s => s[typeof(StateItem).FullName])
                .Return(null);

            var state = this.Sut.Load<StateItem>();

            Assert.That(state, Is.Not.Null);
            Assert.That(state.Name, Is.Null);
            Assert.That(state.Prices, Is.Not.Null);
            Assert.That(state.Prices, Is.Empty);
        }

        [Test]
        public void Load_non_existing_state_should_create_via_factory_method() {
            this.SessionMock.Expect(s => s[typeof(StateItem).FullName])
                .Return(null);

            var state = this.Sut.Load<StateItem>(() => new StateItem("Warenkorb"));

            Assert.That(state, Is.Not.Null);
            Assert.That(state.Name, Is.EqualTo("Warenkorb"));
            Assert.That(state.Prices, Is.Not.Null);
            Assert.That(state.Prices, Is.Empty);
        }

        [Test]
        public void Load_saved_state_should_return_saved_state() {
            Assert.Ignore("Todo,jac: Überprüfen, was genau mit den Mocks nicht stimmt.");

            var state = new StateItem {
                Name = "Warenkorb",
                Prices = new List<int> { 2, 3, 5, 7 }
            };

            this.SessionMock.Expect(s => s[typeof(StateItem).FullName])
                .Return(state);

            var loaded = this.Sut.Load<StateItem>();

            Assert.That(loaded, Is.Not.Null);
            Assert.That(loaded.Name, Is.EqualTo("Warenkorb"));
            Assert.That(loaded.Prices, Is.EquivalentTo(state.Prices));
        }

        [Test]
        public void Save_state() {
            var state = new StateItem {
                Prices = new List<int> { 2, 3, 5, 7 }
            };

            this.SessionMock.Expect(s => s[typeof(StateItem).FullName] = state);

            this.Sut.Save<StateItem>(state);
        }

        [Test]
        public void Save_state_providing_no_state_object_should_throw() {
            var ex = Assert.Throws<ArgumentNullException>(() => this.Sut.Save<StateItem>(null));
            Assert.That(ex.ParamName, Is.EqualTo("state"));
        }

        [Test]
        public void Clear_saved_state() {
            this.SessionMock.Expect(s => s.Remove(typeof(StateItem).FullName));

            this.Sut.Remove<StateItem>();
        }

        // state under test
        private class StateItem {
            public StateItem() {
                this.Prices = new List<int>();
            }

            public StateItem(string name)
                : this() {

                this.Name = name;
            }

            public IList<int> Prices { get; set; }

            public string Name { get; set; }
        }
    }
}
