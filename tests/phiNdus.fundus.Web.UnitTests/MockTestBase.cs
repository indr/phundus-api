using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Commons;
using Rhino.Mocks;

namespace phiNdus.fundus.TestHelpers {
    using phiNdus.fundus.Domain;
    using piNuts.phundus.Infrastructure;

    public abstract class MockTestBase<TSut> {

        protected MockRepository MockFactory { get; private set; }

        /// <summary>
        /// Liefert die Instanz des zu testenden Systems.
        /// </summary>
        protected TSut Sut { get; private set; }

        [SetUp]
        public virtual void Setup() {
            this.MockFactory = new MockRepository();

            var container = new WindsorContainer();
            GlobalContainer.Initialize(container);

            this.RegisterDependencies(container);

            this.Sut = this.CreateSut();
        }

        /// <summary>
        /// Über diese Methode können für die einzelnen Testfälle die entsprechenden Dependencies
        /// registriert werden. Der Container wird über GlobalContainer.Initialize gesetzt.
        /// </summary>
        protected virtual void RegisterDependencies(IWindsorContainer container) {
        }

        /// <summary>
        /// Diese Methode soll eine Instanz der zu testenden Komponente liefern. Diese Methode
        /// wird vor jedem Test aufgerufen, nachdem die MockFactory zur Verfügung steht und die
        /// Dependencies registriert wurden.
        /// </summary>
        protected abstract TSut CreateSut();



        protected T GenerateAndRegisterStub<T>() where T : class
        {
            var result = MockRepository.GenerateStub<T>();
            GlobalContainer.Container.Register(Component.For<T>().Instance(result));
            return result;
        }
    }
}
