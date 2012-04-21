using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace phiNdus.fundus.Core.Web.State {
    public interface IStateManager {

        /// <summary>
        /// Versucht das State-Item vom übergebenen Typ zu laden. Wenn es noch
        /// nicht vorhanden ist, wird via new TState() ein neues angelegt .
        /// </summary>
        TState Load<TState>() where TState : class, new();

        /// <summary>
        /// Versucht das State-Item vom übergebenen Typ zu laden. Wenn es noch
        /// nicht vorhanden ist, wird via <param name="factoryMethod">factoryMethod</param> 
        /// ein neues angelegt.
        /// </summary>
        TState Load<TState>(Func<TState> factoryMethod) where TState : class;

        /// <summary>
        /// Speichert das übergebene Item im State. Pro Typ kann nur eine Instanz gespeichert
        /// werden.
        /// </summary>
        void Save<TState>(TState state);

        /// <summary>
        /// Löscht alle State-Informationen für den übergebenen Typ.
        /// </summary>
        void Remove<TState>();
    }
}
