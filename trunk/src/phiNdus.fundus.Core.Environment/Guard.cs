using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace phiNdus.fundus.Core.Environment {
    public static class Guard {

        /// <summary>
        /// Wenn das übergebene <paramref name="predicate"/> <c>true</c> ist, wird eine Exception vom Typ
        /// <paramref name="TException"/> ausgelöst.
        /// </summary>
        /// <typeparam name="TException">Typ der auszulösenden Exception.</typeparam>
        /// <param name="predicate">Bedingung, die geprüft werden soll und entscheidt, ob eine Exception 
        /// ausgelöst werden soll.</param>
        /// <param name="message">Die Nachricht, welche dem Konstruktor der Exception übergeben werden soll.</param>
        public static void Against<TException>(bool predicate, string message) where TException : Exception {
            if (predicate) {
                throw (TException)Activator.CreateInstance(typeof(TException), message);
            }
        }

        /// <summary>
        /// Wenn das übergebene <paramref name="predicate"/> <c>true</c> ist, wird eine Exception vom Typ
        /// <paramref name="TException"/> ausgelöst.
        /// </summary>
        /// <typeparam name="TException">Typ der auszulösenden Exception.</typeparam>
        /// <param name="predicate">Bedingung, die geprüft werden soll und entscheidt, ob eine Exception 
        /// ausgelöst werden soll.</param>
        /// <param name="message">Die Nachricht, welche dem Konstruktor der Exception übergeben werden soll.</param>
        /// <param name="args">Die Argumente, mit welchen die Nachricht formatiert werden soll.</param>
        public static void Against<TException>(bool predicate, string message, params object[] args) where TException : Exception {
            Guard.Against<TException>(predicate, string.Format(message, args));
        }
    }
}
