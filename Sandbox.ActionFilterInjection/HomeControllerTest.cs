using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Filters;
using Sandbox.ActionFilterInjection.Filters;
using Sandbox.ActionFilterInjection.Services;
using Xunit;

namespace Sandbox.ActionFilterInjection
{
    public class HomeControllerTest
    {
        [Fact]
        public void expect_headers()
        {
            var server = GetApiServer();

            using (var client = new HttpMessageInvoker(server))
            {
                using (var request = CreateGet("Home/Get"))
                using (var response = client.SendAsync(request, CancellationToken.None).Result)
                {
                    Assert.Equal(2, response.Headers.Count());
                    Assert.Equal(new[] {"OneValue"}, response.Headers.GetValues("One"));
                    Assert.Equal(new[] {"TwoValue"}, response.Headers.GetValues("Two"));
                }
            }
        }

        static HttpRequestMessage CreateGet(string url)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get, string.Concat("http://localhost/", url));

            return request;
        }

        static HttpServer GetApiServer()
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(name: "DefaultApi",
                                       routeTemplate: "{controller}/{action}",
                                       defaults: new {id = RouteParameter.Optional});

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Services.Replace(
                typeof (IFilterProvider), new ServiceFilterProvider(Activator.CreateInstance));

            var server = new HttpServer(config);

            return server;
        }
    }
}