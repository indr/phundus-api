namespace Phundus.Core.Specs.Supports
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

        public object On(Type type)
        {
            object instance;
            if (_dependencies.TryGetValue(type, out instance))
                return instance;

            instance = MockRepository.GenerateStub(type);
            _dependencies.Add(type, instance);
            return instance;
        }
    }
}