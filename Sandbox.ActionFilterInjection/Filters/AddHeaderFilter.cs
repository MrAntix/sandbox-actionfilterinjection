using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Sandbox.ActionFilterInjection.Services;

namespace Sandbox.ActionFilterInjection.Filters
{
    public class AddHeaderFilter : IActionFilter, IService<AddHeaderAttribute>
    {
        public IEnumerable<AddHeaderAttribute> Attributes { get; set; }

        public bool AllowMultiple
        {
            get { return true; }
        }

        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(
            HttpActionContext actionContext, CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            var response = await continuation();

            foreach (var attribute in Attributes)
                response.Headers.Add(attribute.Name, attribute.Value);

            return response;
        }
    }
}