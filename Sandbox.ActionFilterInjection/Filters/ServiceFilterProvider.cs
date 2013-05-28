using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Sandbox.ActionFilterInjection.Services;

namespace Sandbox.ActionFilterInjection.Filters
{
    public class ServiceFilterProvider : IFilterProvider
    {
        readonly ServiceResolver _resolver;

        public ServiceFilterProvider(Func<Type, object> resolve)
        {
            _resolver = new ServiceResolver(resolve);
        }

        public IEnumerable<FilterInfo> GetFilters(
            HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            // Taken from ActionDescriptorFilterProvider
            var controllerFilters
                = GetFilterInfos(actionDescriptor.ControllerDescriptor.GetFilters(), FilterScope.Controller);
            var actionFilters
                = GetFilterInfos(actionDescriptor.GetFilters(), FilterScope.Action);
            // Taken from ActionDescriptorFilterProvider

            var controllerProxiedFilters
                = GetFilterInfos(actionDescriptor.ControllerDescriptor.GetCustomAttributes<IServiceAttribute>(),
                                 FilterScope.Controller);
            var actionProxiedFilters
                = GetFilterInfos(actionDescriptor.GetCustomAttributes<IServiceAttribute>(), FilterScope.Action);

            return controllerFilters
                .Concat(controllerProxiedFilters)
                .Concat(actionFilters)
                .Concat(actionProxiedFilters);
        }

        static IEnumerable<FilterInfo> GetFilterInfos(
            IEnumerable<IFilter> attributes, FilterScope scope)
        {
            return attributes.Select(i => new FilterInfo(i, scope));
        }

        IEnumerable<FilterInfo> GetFilterInfos(
            IEnumerable<IServiceAttribute> allAttributes, FilterScope scope)
        {
            return _resolver
                .Resolve<IFilter>(allAttributes)
                .Select(f => new FilterInfo(f, scope));
        }
    }
}