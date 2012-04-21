using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace phiNdus.fundus.Web.State {
    public class SessionStateManager : IStateManager {

        public SessionStateManager() {
            this.Session = Rhino.Commons.IoC.Resolve<HttpContextBase>().Session;
        }

        private HttpSessionStateBase Session { get; set; }

        public TState Load<TState>() where TState : class, new() {
            return Load<TState>(() => new TState());
        }

        public TState Load<TState>(Func<TState> factoryMethod) where TState : class {
            var state = this.Session[CreateKey<TState>()] as TState;

            if (state == null) {
                state = factoryMethod();
            }

            return state;
        }

        public void Save<TState>(TState state) {
            Rhino.Commons.Guard.Against<ArgumentNullException>(state == null, "state");

            this.Session[CreateKey<TState>()] = state;
        }

        public void Remove<TState>() {
            this.Session.Remove(CreateKey<TState>());
        }

        private static string CreateKey<TState>() {
            return typeof(TState).FullName;
        }
    }
}