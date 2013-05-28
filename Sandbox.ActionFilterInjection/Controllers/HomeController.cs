using System.Net;
using System.Net.Http;
using System.Web.Http;
using Sandbox.ActionFilterInjection.Filters;

namespace Sandbox.ActionFilterInjection.Controllers
{
    public class HomeController : ApiController
    {
        [AddHeader("One", "OneValue")]
        [AddHeader("Two", "TwoValue")]
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}