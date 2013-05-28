using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sandbox.ActionFilterInjection.Services
{
    public class ServiceResolver
    {
        readonly Func<Type, object> _resolve;
        readonly MethodInfo _setAttributesMethod;

        public ServiceResolver(Func<Type, object> resolve)
        {
            _resolve = resolve;
            _setAttributesMethod = GetType()
                .GetMethod("SetAttributes", BindingFlags.Static | BindingFlags.NonPublic);
        }

        public IEnumerable<T> Resolve<T>(
            IEnumerable<IServiceAttribute> allAttributes)
        {
            foreach (var group in allAttributes.GroupBy(p => p.ServiceType))
            {
                var filter = (T)_resolve(group.Key);
                var attributes = group.ToArray();

                _setAttributesMethod
                    .MakeGenericMethod(attributes.First().GetType())
                    .Invoke(null, new object[] { filter, attributes });

                yield return filter;
            }
        }

        static void SetAttributes<T>(
            object service,
            IEnumerable<IServiceAttribute> attributes)
            where T : IServiceAttribute
        {
            var real = service as IService<T>;
            if (real != null)
                real.Attributes = attributes.Cast<T>().ToArray();
        }
    }
}