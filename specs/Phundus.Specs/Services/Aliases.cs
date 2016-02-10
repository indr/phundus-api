namespace Phundus.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class Aliases<T>
    {
        private readonly IDictionary<string, T> _collection = new Dictionary<string, T>();

        public T this[string alias]
        {
            get
            {
                if (alias == null)
                    return default(T);
                T result;
                if (!_collection.TryGetValue(alias, out result))
                    throw new InvalidOperationException(String.Format("Alias {0} unknown.", alias));
                return result;
            }
            set
            {
                if (alias != "")
                    Debug.WriteLine("Aliased {0} to {1}.", alias, value);
                _collection[alias] = value;
            }
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool ContainsAlias(string alias)
        {
            return _collection.ContainsKey(alias);
        }

        public bool TryGetValue(string alias, out T value)
        {
            return _collection.TryGetValue(alias, out value);
        }
    }
}