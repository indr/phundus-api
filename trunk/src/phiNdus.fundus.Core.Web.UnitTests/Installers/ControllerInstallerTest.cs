using System;
using System.Linq;
using NUnit.Framework;
using Castle.Windsor;
using Castle.MicroKernel;
using System.Web.Mvc;
using Castle.Core;
using phiNdus.fundus.Core.Web.Installers;

namespace phiNdus.fundus.Core.Web.UnitTests.Installers {

    [TestFixture]
    public class ControllerInstallerTest {

        // Diese Tests wurden kopiert von http://docs.castleproject.org/Windsor.Windsor-tutorial-part-three-a-testing-your-first-installer.ashx

        private IWindsorContainer ContainerWithControllers { get; set; }

        [TestFixtureSetUp]
        public void Setup() {
            this.ContainerWithControllers = new WindsorContainer()
                .Install(new ControllerInstaller());
        }

        [Test]
        public void All_controllers_implement_IController() {

            Assert.Ignore("Funktioniert erst, wenn ein Controller existiert im 'web'-Projekt");

            var allHandlers = GetAllHandlers(this.ContainerWithControllers);
            var controllerHandlers = GetHandlersFor(typeof(IController), this.ContainerWithControllers);

            CollectionAssert.IsNotEmpty(allHandlers);
            CollectionAssert.AreEqual(allHandlers, controllerHandlers);
        }

        [Test]
        public void All_controllers_are_registered() {
            // Is<TType> is an helper, extension method from Windsor
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            //var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
            var allControllers = GetPublicClassesFromApplicationAssembly(c => typeof(IController).IsAssignableFrom(c));            
            var registeredControllers = GetImplementationTypesFor(typeof(IController), this.ContainerWithControllers);
            CollectionAssert.AreEqual(allControllers, registeredControllers);
        }

        [Test]
        public void All_and_only_controllers_have_Controllers_suffix() {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
            var registeredControllers = GetImplementationTypesFor(typeof(IController), this.ContainerWithControllers);
            CollectionAssert.AreEqual(allControllers, registeredControllers);
        }

        [Test]
        public void All_and_only_controllers_live_in_Controllers_namespace() {
            var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
            var registeredControllers = GetImplementationTypesFor(typeof(IController), this.ContainerWithControllers);
            CollectionAssert.AreEqual(allControllers, registeredControllers);
        }

        [Test]
        public void All_controllers_are_transient() {
            var nonTransientControllers = GetHandlersFor(typeof(IController), this.ContainerWithControllers)
                .Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();

            CollectionAssert.IsEmpty(nonTransientControllers);
        }

        [Test]
        public void All_controllers_expose_themselves_as_service() {
            var controllersWithWrongName = GetHandlersFor(typeof(IController), this.ContainerWithControllers)
                .Where(controller => controller.Service != controller.ComponentModel.Implementation)
                .ToArray();

            CollectionAssert.IsEmpty(controllersWithWrongName);
        }

        //=========================================================================================
        #region Helper Methods
        // see: https://github.com/kkozmic/ToBeSeen/blob/a88873244952ca55857e0ff99e16a428f21ab83c/tst/ToBeSeenTests/ControllersInstallerTests.cs

        private IHandler[] GetAllHandlers(IWindsorContainer container) {
            return GetHandlersFor(typeof(object), container);
        }

        private IHandler[] GetHandlersFor(Type type, IWindsorContainer container) {
            return container.Kernel.GetAssignableHandlers(type);
        }

        private Type[] GetImplementationTypesFor(Type type, IWindsorContainer container) {
            return GetHandlersFor(type, container)
            .Select(h => h.ComponentModel.Implementation)
            .OrderBy(t => t.Name)
            .ToArray();
        }

        private Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where) {
            //return typeof(HomeController).Assembly.GetExportedTypes()
            return typeof(ControllerInstaller).Assembly.GetExportedTypes()
            .Where(t => t.IsClass)
            .Where(t => t.IsAbstract == false)
            .Where(where.Invoke)
            .OrderBy(t => t.Name)
            .ToArray();
        }

        #endregion
        //=========================================================================================
    }
}
