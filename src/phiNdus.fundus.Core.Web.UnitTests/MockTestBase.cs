﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Castle.Windsor;
using Rhino.Commons;

namespace phiNdus.fundus.Core.Web.UnitTests {
    public abstract class MockTestBase<TSut> {

        protected MockRepository MockFactory { get; private set; }

        /// <summary>
        /// Liefert die Instanz des zu testenden Systems.
        /// </summary>
        protected TSut Sut { get; private set; }

        [SetUp]
        public void Setup() {
            this.MockFactory = new MockRepository();

            var container = new WindsorContainer();
            IoC.Initialize(container);

            this.RegisterDependencies(container);

            this.Sut = this.CreateSut();
        }

        /// <summary>
        /// Über diese Methode können für die einzelnen Testfälle die entsprechenden Dependencies
        /// registriert werden. Der Container wird über IoC.Initialize gesetzt.
        /// </summary>
        protected virtual void RegisterDependencies(IWindsorContainer container) {
        }

        /// <summary>
        /// Diese Methode soll eine Instanz der zu testenden Komponente liefern. Diese Methode
        /// wird vor jedem Test aufgerufen, nachdem die MockFactory zur Verfügung steht und die
        /// Dependencies registriert wurden.
        /// </summary>
        protected abstract TSut CreateSut();
    }
}
