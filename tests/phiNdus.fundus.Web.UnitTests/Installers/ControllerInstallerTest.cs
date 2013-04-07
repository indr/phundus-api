namespace phiNdus.fundus.Web.UnitTests.Installers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using App_Start.Installers;
    using Castle.Core;
    using Castle.MicroKernel;
    using Castle.Windsor;
    using NUnit.Framework;
    using Web.Controllers;

    [TestFixture]
    public class ControllerInstallerTest
    {
        // Diese Tests wurden kopiert von http://docs.castleproject.org/Windsor.Windsor-tutorial-part-three-a-testing-your-first-installer.ashx

        IWindsorContainer ContainerWithControllers { get; set; }

        [TestFixtureSetUp]
        public void Setup()
        {
            ContainerWithControllers = new WindsorContainer()
                .Install(new ControllerInstaller());
        }


        // see: https://github.com/kkozmic/ToBeSeen/blob/a88873244952ca55857e0ff99e16a428f21ab83c/tst/ToBeSeenTests/ControllersInstallerTests.cs

        IHandler[] GetAllHandlers(IWindsorContainer container)
        {
            return GetHandlersFor(typeof (object), container);
        }

        IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
        {
            return container.Kernel.GetAssignableHandlers(type);
        }

        Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
        {
            return GetHandlersFor(type, container)
                .Select(h => h.ComponentModel.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }

        Type[] GetPublicClassesFromApplicationAssembly(Predicate<Type> where)
        {
            return typeof (HomeController).Assembly.GetExportedTypes()
                                          .Where(t => t.IsClass)
                                          .Where(t => t.IsAbstract == false)
                                          .Where(where.Invoke)
                                          .OrderBy(t => t.Name)
                                          .ToArray();
        }

        //[Test]
        //public void All_and_only_controllers_have_Controllers_suffix()
        //{
        //    var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Name.EndsWith("Controller"));
        //    var registeredControllers = GetImplementationTypesFor(typeof (IController), ContainerWithControllers);
        //    CollectionAssert.AreEqual(allControllers, registeredControllers);
        //}

        //[Test]
        //public void All_and_only_controllers_live_in_Controllers_namespace()
        //{
        //    var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Namespace.Contains("Controllers"));
        //    var registeredControllers = GetImplementationTypesFor(typeof (IController), ContainerWithControllers);
        //    CollectionAssert.AreEqual(allControllers, registeredControllers);
        //}

        [Test]
        public void All_controllers_are_registered()
        {
            // Is<TType> is an helper, extension method from Windsor
            // which behaves like 'is' keyword in C# but at a Type, not instance level
            //var allControllers = GetPublicClassesFromApplicationAssembly(c => c.Is<IController>());
            var allControllers = GetPublicClassesFromApplicationAssembly(c => typeof (IController).IsAssignableFrom(c));
            var registeredControllers = GetImplementationTypesFor(typeof (IController), ContainerWithControllers);
            CollectionAssert.AreEqual(allControllers, registeredControllers);
        }

        [Test]
        public void All_controllers_are_transient()
        {
            var nonTransientControllers = GetHandlersFor(typeof (IController), ContainerWithControllers)
                .Where(controller => controller.ComponentModel.LifestyleType != LifestyleType.Transient)
                .ToArray();

            CollectionAssert.IsEmpty(nonTransientControllers);
        }

        [Test]
        public void All_controllers_expose_themselves_as_service()
        {
            //var controllersWithWrongName = GetHandlersFor(typeof(IController), this.ContainerWithControllers)
            //    .Where(controller => controller.Service != controller.ComponentModel.Implementation)
            //    .ToArray();

            //CollectionAssert.IsEmpty(controllersWithWrongName);
        }

        //[Test]
        //public void All_controllers_implement_IController()
        //{
        //    var allHandlers = GetAllHandlers(ContainerWithControllers);
        //    var controllerHandlers = GetHandlersFor(typeof (IController), ContainerWithControllers);

        //    CollectionAssert.IsNotEmpty(allHandlers);
        //    CollectionAssert.AreEqual(allHandlers, controllerHandlers);
        //}
    }
}