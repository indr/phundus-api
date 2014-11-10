namespace Phundus.Core.Specs.Supports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class Container
    {
        private readonly Depends _depends;

        public Container()
        {
            _depends = new Depends();
        }

        public Depends Depend
        {
            get { return _depends; }
        }

        public T Resolve<T>()
        {
            var instance = CreateInstanceWithConstructorInjection<T>();
            InjectProperties(instance);
            return instance;
        }

        private void InjectProperties<T>(T instance)
        {
            var properties =
                typeof (T).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (var eachProperty in properties)
            {
                eachProperty.SetValue(instance, Depend.On(eachProperty.PropertyType), null);
            }
        }

        private T CreateInstanceWithConstructorInjection<T>()
        {
            var ctor = GetGreediestConstructor(typeof (T));
            var parameterValues = GenerateParameterValues(ctor);

            var result = (T) ctor.Invoke(parameterValues);
            return result;
        }

        private ConstructorInfo GetGreediestConstructor(Type type)
        {
            return type.GetConstructors().OrderByDescending(k => k.GetParameters().Length).First();
        }

        private object[] GenerateParameterValues(ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters();
            var parameterValues = new List<object>();
            foreach (var eachParameter in parameters)
            {
                parameterValues.Add(Depend.On(eachParameter.ParameterType));
            }
            return parameterValues.ToArray();
        }
    }
}