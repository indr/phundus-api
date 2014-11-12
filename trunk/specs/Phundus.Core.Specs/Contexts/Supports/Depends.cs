namespace Phundus.Core.Specs.Contexts
{
    using System;
    using System.Collections.Generic;
    using Rhino.Mocks;

    public class Depends
    {
        private readonly IDictionary<Type, Object> _dependencies = new Dictionary<Type, object>();

        public TDependency On<TDependency>() where TDependency : class
        {
            return (TDependency) On(typeof (TDependency));
        }

        public TDependency On<TDependency>(object instance) where TDependency : class
        {
            return (TDependency) On(typeof (TDependency), instance);
        }

        public object On(Type type)
        {
            return On(type, () => MockRepository.GenerateStub(type));
        }

        public object On(Type type, object instance)
        {
            return On(type, () => instance);
        }

        private object On(Type type, Func<object> instanceFactory)
        {
            object instance;
            if (_dependencies.TryGetValue(type, out instance))
                return instance;

            instance = instanceFactory();
            _dependencies.Add(type, instance);
            return instance;
        }
    }
}