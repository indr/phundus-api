namespace Phundus.Common
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class RedirectToHandle
    {
        private static readonly MethodInfo InternalPreserveStackTraceMethod =
            typeof (Exception).GetMethod("InternalPreserveStackTrace", BindingFlags.Instance | BindingFlags.NonPublic);

        private static readonly IDictionary<Type, IDictionary<Type, MethodInfo>> Caches =
            new ConcurrentDictionary<Type, IDictionary<Type, MethodInfo>>();

        public static void InvokeEventOptional(object instance, object e)
        {
            var cache = GetCache(instance.GetType());

            MethodInfo info = null;
            var type = e.GetType();
            while (type != null && type != typeof(Object))
            {
                if (cache.TryGetValue(type, out info))
                    break;
                type = type.BaseType;
            }

            if (info == null)
                return;

            try
            {
                info.Invoke(instance, new[] {e});
            }
            catch (TargetInvocationException ex)
            {
                if (null != InternalPreserveStackTraceMethod)
                    InternalPreserveStackTraceMethod.Invoke(ex.InnerException, new object[0]);
                throw ex.InnerException;
            }
        }

        private static IDictionary<Type, MethodInfo> GetCache(Type type)
        {
            IDictionary<Type, MethodInfo> infos;
            if (Caches.TryGetValue(type, out infos))
                return infos;

            lock (Caches)
            {
                if (Caches.TryGetValue(type, out infos))
                    return infos;

                infos = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(m => m.Name == "Handle")
                    .Where(m => m.GetParameters().Length == 1)
                    .ToDictionary(m => m.GetParameters().First().ParameterType, m => m);

                Caches.Add(type, infos);
                return infos;
            }
        }
    }
}